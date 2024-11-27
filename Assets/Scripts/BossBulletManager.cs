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

    [Header("Sounds")]
    [SerializeField] private AudioClip explosionClip;

    public void Initialize(Vector3 _targetPosition, PlayerHpManager _playerHpManager)
    {
        // ˆÚ“®‚·‚é
        transform.DOMove(_targetPosition, moveTime).SetEase(Ease.Linear).OnComplete(DestroySelf);

        // ‰ñ“]‚·‚é
        transform.DORotate(new Vector3(Random.Range(-1,1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 360f, 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        // •Ï”æ“¾
        playerHpManager = _playerHpManager;
        playerTransform = playerHpManager.transform;
    }
    void DestroySelf()
    {
        // Effect‚ÌeqŠÖŒW‚ğ‰ğœ‚·‚é
        bulletParticle.transform.parent = null;

        // ‰¹‚ğ–Â‚ç‚·
        AudioSource.PlayClipAtPoint(explosionClip, transform.position);

        // ParticleSystem‚Ì’â~
        bulletParticle.Stop();

        // DOTween‚Ì’â~
        DOTween.Kill(gameObject);

        // Á–Å‚³‚¹‚é
        Destroy(gameObject);
    }

    void Update()
    {
        // Player‚ÉUŒ‚
        Vector3 playerRePosition = playerTransform.position;

        if (IsHitObject(ref playerRePosition, 0.5f))
        {
            // Damage‚ğ—^‚¦‚é
            playerHpManager.Damage();

            // Á–Å‚·‚é
            DestroySelf();
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition, float _range)
    {
        // ‚‚³‚ğPlayer‚É‡‚í‚¹‚½VÀ•W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // ‹——£‚ğæ“¾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // ˆê’è‹——£“à‚ÌƒIƒuƒWƒFƒNƒg‚ğUŒ‚‚·‚é
        if (distance < _range)
        {
            return true;
        }
        return false;
    }
}
