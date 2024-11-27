using UnityEngine;

public class PlayerSlashManager : MonoBehaviour
{
    // PlayerComponent
    private PlayerMoveManager moveManager;

    // 受け取る変数
    private float adjustDistance;

    // フラグ類
    private bool isPowerUp;

    [Header("Attack Parameter")]
    [SerializeField] private float damageValue;

    [Header("Slash")]
    [SerializeField] private float slashRange;

    [Header("EffectPrefabs")]
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject bossSlashHitPrefab;
    [SerializeField] private GameObject godRayPrefab;

    [Header("Sounds")]
    [SerializeField] private AudioClip koubou;

    // Other Objects
    private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    public void Initialize(PlayerMoveManager _moveManager, Transform _bossCoreTransform, float _adjustDistance, bool _isPowerUp)
    {
        // 変数を受け取る
        moveManager = _moveManager;
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();
        adjustDistance = _adjustDistance;
        isPowerUp = _isPowerUp;

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

                // パワーアップ状態だと柱を壊し、光芒を出現させる
                if (isPowerUp)
                {
                    // 音を鳴らす
                    AudioSource.PlayClipAtPoint(koubou, transform.position);

                    // 光芒を出現させる
                    Instantiate(godRayPrefab, new(pillarRePosition.x, 0f, pillarRePosition.z), Quaternion.identity);

                }
                // 柱を破壊する
                Destroy(pillar);

                break;
            }
        }

        // Lightに攻撃
        foreach (GameObject Light in GameObject.FindGameObjectsWithTag("Light"))
        {
            Vector3 lightRePosition = Light.transform.position;

            if (IsHitObject(ref lightRePosition))
            {
                Vector3 toPlayer = Vector3.Normalize(transform.position - lightRePosition);
                Vector3 diffVector = toPlayer * adjustDistance;

                // HitEffect作成
                Instantiate(hitPrefab, lightRePosition + diffVector, Quaternion.identity);

                // 反動
                moveManager.Reaction(toPlayer);

                break;
            }
        }

        // BossCoreに攻撃
        Vector3 bossCoreRePosition = bossCoreTransform.position;

        if (IsHitObject(ref bossCoreRePosition) && bossCoreManager.GetCanHit())
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
