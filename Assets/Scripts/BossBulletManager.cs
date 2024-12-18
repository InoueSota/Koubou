using DG.Tweening;
using UnityEngine;

public class BossBulletManager : MonoBehaviour
{
    // OtherTransform
    private PlayerHpManager playerHpManager;
    private Transform playerTransform;

    [Header("Parameter")]
    [SerializeField] private float moveTime;
    [SerializeField] private ParticleSystem bulletParticle;

    public void Initialize(Vector3 _targetPosition, PlayerHpManager _playerHpManager)
    {
        // 移動する
        transform.DOMove(_targetPosition, moveTime).SetEase(Ease.Linear).OnComplete(DestroySelf);

        // 回転する
        transform.DORotate(new Vector3(Random.Range(-1,1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 360f, 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        // 変数取得
        playerHpManager = _playerHpManager;
        playerTransform = playerHpManager.transform;
    }
    void DestroySelf()
    {
        // Effectの親子関係を解除する
        bulletParticle.transform.parent = null;

        // ParticleSystemの停止
        bulletParticle.Stop();

        // DOTweenの停止
        DOTween.Kill(gameObject);

        // 消滅させる
        Destroy(gameObject);
    }

    void Update()
    {
        // Playerに攻撃
        Vector3 playerRePosition = playerTransform.position;

        if (IsHitObject(ref playerRePosition, 0.5f))
        {
            // Damageを与える
            playerHpManager.Damage();

            // 消滅する
            DestroySelf();
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition, float _range)
    {
        // 高さをPlayerに合わせた新座標
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // 距離を取得
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // 一定距離内のオブジェクトを攻撃する
        if (distance < _range)
        {
            return true;
        }
        return false;
    }
}
