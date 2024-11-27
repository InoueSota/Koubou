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
        // �ړ�����
        transform.DOMove(_targetPosition, moveTime).SetEase(Ease.Linear).OnComplete(DestroySelf);

        // ��]����
        transform.DORotate(new Vector3(Random.Range(-1,1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 360f, 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        // �ϐ��擾
        playerHpManager = _playerHpManager;
        playerTransform = playerHpManager.transform;
    }
    void DestroySelf()
    {
        // Effect�̐e�q�֌W����������
        bulletParticle.transform.parent = null;

        // ����炷
        AudioSource.PlayClipAtPoint(explosionClip, transform.position);

        // ParticleSystem�̒�~
        bulletParticle.Stop();

        // DOTween�̒�~
        DOTween.Kill(gameObject);

        // ���ł�����
        Destroy(gameObject);
    }

    void Update()
    {
        // Player�ɍU��
        Vector3 playerRePosition = playerTransform.position;

        if (IsHitObject(ref playerRePosition, 0.5f))
        {
            // Damage��^����
            playerHpManager.Damage();

            // ���ł���
            DestroySelf();
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition, float _range)
    {
        // ������Player�ɍ��킹���V���W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // �������擾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // ��苗�����̃I�u�W�F�N�g���U������
        if (distance < _range)
        {
            return true;
        }
        return false;
    }
}
