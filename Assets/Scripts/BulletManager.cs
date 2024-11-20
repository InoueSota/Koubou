using DG.Tweening;
using UnityEngine;

public class BulletManager : MonoBehaviour
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
    [SerializeField] private GameObject hitPrefab;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed;

    // OtherObjects
    private Transform bossCoreTransform;
    private BossCoreManager bossCoreManager;

    public void Initialize(Transform _bossCoreTransform, Vector3 _moveVector, float _adjustDistance)
    {
        // ‰ñ“]‚·‚é
        transform.DORotate(Vector3.right * 360f, 0.4f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Restart);

        // BossCore‚ğæ“¾
        bossCoreTransform = _bossCoreTransform;
        bossCoreManager = bossCoreTransform.GetComponent<BossCoreManager>();

        // ˆÚ“®•ûŒü‚ğæ“¾
        moveVector = Vector3.Normalize(_moveVector);

        // ‚¸‚ç‚·‹——£‚ğæ“¾
        adjustDistance = _adjustDistance;

        // ¶‘¶ŠÔ‚Ìİ’è
        lifeTimer = lifeTime;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0) { DestroySelf(); }

        // ˆÚ“®
        transform.position += moveVector * (moveSpeed * Time.deltaTime);

        // ‰ñ“]
        transform.Rotate(new());

        // Pillar‚ÉUŒ‚
        foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
        {
            Vector3 pillarRePosition = pillar.transform.position;

            if (IsHitObject(ref pillarRePosition))
            {
                Vector3 diffVector = -moveVector * adjustDistance;

                // HitEffectì¬
                Instantiate(hitPrefab, pillarRePosition + diffVector, Quaternion.identity);

                // Á–Å‚·‚é
                DestroySelf();
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

            // Á–Å‚·‚é
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        // Effect‚ÌeqŠÖŒW‚ğ‰ğœ‚·‚é
        bulletParticle.transform.parent = null;

        // ParticleSystem‚Ì’â~
        bulletParticle.Stop();

        // DOTween‚Ì’â~
        DOTween.Kill(gameObject);

        // Á–Å
        Destroy(gameObject);
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // ‚‚³‚ğPlayer‚É‡‚í‚¹‚½VÀ•W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // ‹——£‚ğæ“¾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // ˆê’è‹——£“à‚ÌƒIƒuƒWƒFƒNƒg‚ğUŒ‚‚·‚é
        if (distance < 0.5f)
        {
            return true;
        }
        return false;
    }
}
