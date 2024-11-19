using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    // Manageræ“¾
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
        // UŒ‚‚ÌƒCƒ“ƒ^[ƒoƒ‹Œv‘ª
        attackIntervalTimer -= Time.deltaTime;
        attackGauge.fillAmount = 1f - attackIntervalTimer / attackIntervalTime;

        // UŒ‚‚ğs‚¤
        if (attackIntervalTimer <= 0f && manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            // Pillar‚ÉUŒ‚
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                Vector3 pillarRePosition = pillar.transform.position;

                if (IsHitObject(ref pillarRePosition))
                {
                    Vector3 toPlayer = Vector3.Normalize(transform.position - pillarRePosition);
                    Vector3 diffVector = toPlayer * adjustDistance;

                    // HitEffectì¬
                    Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);

                    // ”½“®
                    moveManager.Reaction(toPlayer);
                }
            }

            // BossCore‚ÉUŒ‚
            Vector3 bossCoreRePosition = bossCoreTransform.position;

            if (IsHitObject(ref bossCoreRePosition))
            {
                Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
                Vector3 diffVector = toPlayer * adjustDistance;

                // HitEffectì¬
                Instantiate(hitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

                // Damage‚ğ—^‚¦‚é
                bossCoreManager.Damage(damageValue);
            }

            // SlashEffectì¬
            Instantiate(slashPrefab, transform.position, Quaternion.identity);

            attackIntervalTimer = attackIntervalTime;
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // ‚‚³‚ğPlayer‚É‡‚í‚¹‚½VÀ•W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // ‹——£‚ğæ“¾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // SlashRange“à‚ÌPillar‚ğUŒ‚‚·‚é
        if (distance < slashRange)
        {
            return true;
        }
        return false;
    }
}
