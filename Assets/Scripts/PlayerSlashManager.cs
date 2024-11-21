using UnityEngine;

public class PlayerSlashManager : MonoBehaviour
{
    // PlayerComponent
    private PlayerMoveManager moveManager;

    // �󂯎��ϐ�
    private float adjustDistance;

    // �t���O��
    private bool isPowerUp;

    [Header("Attack Parameter")]
    [SerializeField] private float damageValue;

    [Header("Slash")]
    [SerializeField] private float slashRange;

    [Header("EffectPrefabs")]
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject bossSlashHitPrefab;
    [SerializeField] private GameObject godRayPrefab;

    // Other Objects
    private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    public void Initialize(PlayerMoveManager _moveManager, Transform _bossCoreTransform, float _adjustDistance, bool _isPowerUp)
    {
        // �ϐ����󂯎��
        moveManager = _moveManager;
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();
        adjustDistance = _adjustDistance;
        isPowerUp = _isPowerUp;

        // �U��
        Attack();
    }

    void Attack()
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

                // ����
                moveManager.Reaction(toPlayer);

                // �p���[�A�b�v��Ԃ��ƒ����󂵁A��䊂��o��������
                if (isPowerUp)
                {
                    // ��䊂��o��������
                    Instantiate(godRayPrefab, new(pillarRePosition.x, 0f, pillarRePosition.z), Quaternion.identity);

                    // ����j�󂷂�
                    Destroy(pillar);
                }

                break;
            }
        }

        // Light�ɍU��
        foreach (GameObject Light in GameObject.FindGameObjectsWithTag("Light"))
        {
            Vector3 lightRePosition = Light.transform.position;

            if (IsHitObject(ref lightRePosition))
            {
                Vector3 toPlayer = Vector3.Normalize(transform.position - lightRePosition);
                Vector3 diffVector = toPlayer * adjustDistance;

                // HitEffect�쐬
                Instantiate(hitPrefab, lightRePosition + diffVector, Quaternion.identity);

                // ����
                moveManager.Reaction(toPlayer);

                break;
            }
        }

        // BossCore�ɍU��
        Vector3 bossCoreRePosition = bossCoreTransform.position;

        if (IsHitObject(ref bossCoreRePosition))
        {
            Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
            Vector3 diffVector = toPlayer * adjustDistance;

            // HitEffect�쐬
            Instantiate(bossSlashHitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

            // Damage��^����
            bossCoreManager.Damage(damageValue);
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
