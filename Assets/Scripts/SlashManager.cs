using UnityEngine;

public class SlashManager : MonoBehaviour
{
    // PlayerComponent
    private PlayerMoveManager moveManager;

    // ó‚¯æ‚é•Ï”
    private float adjustDistance;

    [Header("Attack Parameter")]
    [SerializeField] private float damageValue;

    [Header("Slash")]
    [SerializeField] private float slashRange;

    [Header("EffectPrefabs")]
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject bossSlashHitPrefab;

    // Other Objects
    private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    public void Initialize(PlayerMoveManager _moveManager, Transform _bossCoreTransform, float _adjustDistance)
    {
        // •Ï”‚ğó‚¯æ‚é
        moveManager = _moveManager;
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();
        adjustDistance = _adjustDistance;

        // UŒ‚
        Attack();
    }

    void Attack()
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
            Instantiate(bossSlashHitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

            // Damage‚ğ—^‚¦‚é
            bossCoreManager.Damage(damageValue);
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
