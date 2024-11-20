using UnityEngine;

public class SlashManager : MonoBehaviour
{
    // PlayerComponent
    private PlayerMoveManager moveManager;

    // 受け取る変数
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
        // 変数を受け取る
        moveManager = _moveManager;
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();
        adjustDistance = _adjustDistance;

        // 攻撃
        Attack();
    }

    void Attack()
    {
        // Pillarに攻撃
        foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
        {
            Vector3 pillarRePosition = pillar.transform.position;

            if (IsHitObject(ref pillarRePosition))
            {
                Vector3 toPlayer = Vector3.Normalize(transform.position - pillarRePosition);
                Vector3 diffVector = toPlayer * adjustDistance;

                // HitEffect作成
                Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);

                // 反動
                moveManager.Reaction(toPlayer);
            }
        }

        // BossCoreに攻撃
        Vector3 bossCoreRePosition = bossCoreTransform.position;

        if (IsHitObject(ref bossCoreRePosition))
        {
            Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
            Vector3 diffVector = toPlayer * adjustDistance;

            // HitEffect作成
            Instantiate(bossSlashHitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

            // Damageを与える
            bossCoreManager.Damage(damageValue);
        }
    }

    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // 高さをPlayerに合わせた新座標
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // 距離を取得
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // SlashRange内のPillarを攻撃する
        if (distance < slashRange)
        {
            return true;
        }
        return false;
    }
}
