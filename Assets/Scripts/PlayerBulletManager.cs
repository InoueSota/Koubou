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
        // ��]����
        transform.DORotate(Vector3.right * 360f, 0.4f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Restart);

        // BossCore���擾
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();

        // �ړ��������擾
        moveVector = Vector3.Normalize(_moveVector);

        // ���炷�������擾
        adjustDistance = _adjustDistance;

        // �������Ԃ̐ݒ�
        lifeTimer = lifeTime;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0) { DestroySelf(); }

        // �ړ�
        transform.position += moveVector * (moveSpeed * Time.deltaTime);

        // ��]
        transform.Rotate(new());

        // Pillar�ɍU��
        foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
        {
            Vector3 pillarRePosition = pillar.transform.position;

            if (IsHitObject(ref pillarRePosition))
            {
                Vector3 diffVector = -moveVector * adjustDistance;

                // HitEffect�쐬
                Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);

                // ���ł���
                DestroySelf();
            }
        }

        // BossCore�ɍU��
        Vector3 bossCoreRePosition = bossCoreTransform.position;

        if (IsHitObject(ref bossCoreRePosition))
        {
            Vector3 toPlayer = Vector3.Normalize(transform.position - bossCoreRePosition);
            Vector3 diffVector = toPlayer * adjustDistance;

            // HitEffect�쐬
            Instantiate(bossBulletHitPrefab, bossCoreRePosition + diffVector, Quaternion.identity);

            // Damage��^����
            bossCoreManager.Damage(damageValue);

            // ���ł���
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        // Effect�̐e�q�֌W����������
        bulletParticle.transform.parent = null;

        // ParticleSystem�̒�~
        bulletParticle.Stop();

        // DOTween�̒�~
        DOTween.Kill(gameObject);

        // ����
        Destroy(gameObject);
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // ������Player�ɍ��킹���V���W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // �������擾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // ��苗�����̃I�u�W�F�N�g���U������
        if (distance < 0.5f)
        {
            return true;
        }
        return false;
    }
}
