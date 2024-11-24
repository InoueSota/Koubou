using DG.Tweening;
using UnityEngine;

public class PlayerBulletManager : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private float damageValue;
    [SerializeField] private float lifeTime;
    private float lifeTimer;
    [SerializeField] private float moveSpeed;
    private Vector3 moveVector;
    private float adjustDistance;

    [Header("Effects")]
    [SerializeField] private ParticleSystem bulletParticle;
    [SerializeField] private GameObject bossBulletHitPrefab;
    [SerializeField] private GameObject hitPrefab;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed;

    // OtherObjects
    private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    public void Initialize(Transform _bossCoreTransform, Vector3 _moveVector, float _adjustDistance)
    {
        // 回転する
        transform.DORotate(Vector3.right * 360f, 0.4f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Restart);

        // BossCoreを取得
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();

        // 移動方向を取得
        moveVector = Vector3.Normalize(_moveVector);

        // ずらす距離を取得
        adjustDistance = _adjustDistance;

        // 生存時間の設定
        lifeTimer = lifeTime;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0) { DestroySelf(); }

        // 移動
        transform.position += moveVector * (moveSpeed * Time.deltaTime);

        // 回転
        transform.Rotate(new());

        // Pillarに攻撃
        foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
        {
            Vector3 pillarRePosition = pillar.transform.position;

            if (IsHitObject(ref pillarRePosition))
            {
                Vector3 diffVector = -moveVector * adjustDistance;

                // HitEffect作成
                Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);

                // 消滅する
                DestroySelf();
            }
        }

        // Lightに攻撃
        foreach (GameObject Light in GameObject.FindGameObjectsWithTag("Light"))
        {
            Vector3 lightRePosition = Light.transform.position;

            if (IsHitObject(ref lightRePosition))
            {
                Vector3 diffVector = -moveVector * adjustDistance;

                // HitEffect作成
                Instantiate(hitPrefab, lightRePosition + diffVector, Quaternion.identity);

                // 消滅する
                DestroySelf();
            }
        }

        // BossCoreに攻撃
        Vector3 bossCoreRePosition = bossCoreTransform.position;

        if (IsHitObject(ref bossCoreRePosition))
        {
            Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
            Vector3 diffVector = toPlayer * adjustDistance;

            // HitEffect作成
            Instantiate(bossBulletHitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

            // Damageを与える
            bossCoreManager.Damage(damageValue);

            // 消滅する
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        // Effectの親子関係を解除する
        bulletParticle.transform.parent = null;

        // ParticleSystemの停止
        bulletParticle.Stop();

        // DOTweenの停止
        DOTween.Kill(gameObject);

        // 消滅
        Destroy(gameObject);
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // 高さをPlayerに合わせた新座標
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // 距離を取得
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // 一定距離内のオブジェクトを攻撃する
        if (distance < 0.5f)
        {
            return true;
        }
        return false;
    }
}
