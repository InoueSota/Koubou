using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    // Manager�擾
    private PlayerManager manager;
    private PlayerMoveManager moveManager;

    [Header("Parameter")]
    [SerializeField] private float damageValue;
    [SerializeField] private float attackIntervalTime;
    private float attackIntervalTimer;

    [Header("Hit")]
    [SerializeField] private float adjustDistance;

    [Header("Slash")]
    [SerializeField] private float slashRange;

    [Header("EffectPrefabs")]
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject slashPrefab;

    [Header("UI")]
    [SerializeField] private Image attackGauge;

    [Header("Other Object")]
    [SerializeField] private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    void Start()
    {
        manager = GetComponent<PlayerManager>();
        moveManager = GetComponent<PlayerMoveManager>();

        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();
    }

    public void ManualUpdate()
    {
        Attack();
    }
    void Attack()
    {
        // �U���̃C���^�[�o���v��
        attackIntervalTimer -= Time.deltaTime;
        attackGauge.fillAmount = 1f - attackIntervalTimer / attackIntervalTime;

        // �U�����s��
        if (attackIntervalTimer <= 0f && manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            // Pillar�ɍU��
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                Vector3 pillarRePosition = pillar.transform.position;

                if (IsHitObject(ref pillarRePosition))
                {
                    Vector3 toPlayer = Vector3.Normalize(transform.position - pillarRePosition);
                    Vector3 diffVector = toPlayer * adjustDistance;

                    // HitEffect�쐬
                    Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);
                }
            }

            // BossCore�ɍU��
            Vector3 bossCoreRePosition = bossCoreTransform.position;

            if (IsHitObject(ref bossCoreRePosition))
            {
                Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
                Vector3 diffVector = toPlayer * adjustDistance;

                // HitEffect�쐬
                Instantiate(hitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

                // Damage��^����
                bossCoreManager.Damage(damageValue);
            }

            // SlashEffect�쐬
            Instantiate(slashPrefab, transform.position, Quaternion.identity);

            attackIntervalTimer = attackIntervalTime;
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // ������Player�ɍ��킹���V���W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // �������擾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // SlashRange����Pillar���U������
        if (distance < slashRange)
        {
            return true;
        }
        return false;
    }
}
