using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    // Manager�擾
    private PlayerManager manager;
    private PlayerMoveManager moveManager;
    private PlayerPowerUpManager powerUpManager;

    [Header("Slash")]
    [SerializeField] private GameObject smallSlashPrefab;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float attackIntervalTime;
    private float attackIntervalTimer;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireIntervalTime;
    private float fireIntervalTimer;

    [Header("Hit")]
    [SerializeField] private float adjustDistance;

    [Header("UI")]
    [SerializeField] private Image attackGauge;
    [SerializeField] private Image fireGauge;

    [Header("Other Object")]
    [SerializeField] private Transform bossCoreTransform;

    [Header("Sounds")]
    [SerializeField] private AudioClip slashClip;
    [SerializeField] private AudioClip fireClip;
    private AudioSource audioSource;

    void Start()
    {
        manager = GetComponent<PlayerManager>();
        moveManager = GetComponent<PlayerMoveManager>();
        powerUpManager = GetComponent<PlayerPowerUpManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ManualUpdate(bool _isPowerUpFrame)
    {
        Slash(_isPowerUpFrame);
        Fire();
    }
    void Slash(bool _isPowerUpFrame)
    {
        // �U���̃C���^�[�o���v��
        attackIntervalTimer -= Time.deltaTime;
        attackGauge.fillAmount = 1f - attackIntervalTimer / attackIntervalTime;

        // �U�����s��
        if (!_isPowerUpFrame && attackIntervalTimer <= 0f && manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            GameObject slash = null;

            // ������Ԃ��ǂ����ɂ���Đ�������slash�̎�ނ�ς���
            if (powerUpManager.GetIsPowerUp())
            {
                // Slash�̐���
                slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
            } else {
                // SmallSlash�̐���
                slash = Instantiate(smallSlashPrefab, transform.position, Quaternion.identity);
            }

            // �ϐ���^����
            slash.GetComponent<PlayerSlashManager>().Initialize(moveManager, bossCoreTransform, adjustDistance, powerUpManager.GetIsPowerUp());

            // ����炷
            audioSource.PlayOneShot(slashClip);

            // �C���^�[�o���̍Đݒ�
            attackIntervalTimer = attackIntervalTime;
        }
    }
    void Fire()
    {
        // �ˌ��̃C���^�[�o���v��
        fireIntervalTimer -= Time.deltaTime;
        fireGauge.fillAmount = 1f - fireIntervalTimer / fireIntervalTime;

        // �ˌ����s��
        if (fireIntervalTimer <= 0f && (manager.GetInputManager().IsPush(manager.GetInputManager().rTrigger) || manager.GetInputManager().IsPush(manager.GetInputManager().yButton)))
        {
            // Bullet�̐���
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // �ړ������i�����l�͐i�s�����j
            Vector3 moveVector = moveManager.GetSaveVector();

            // �_�����߂Ă�����ړ��������{�X�Ɍ�����
            if (manager.GetInputManager().IsPush(manager.GetInputManager().lTrigger))
            {
                moveVector = bossCoreTransform.position - transform.position;
            }

            // Bullet�Ɉړ���������
            bullet.GetComponent<PlayerBulletManager>().Initialize(bossCoreTransform, moveVector, adjustDistance, powerUpManager.GetIsPowerUp());

            // ����炷
            audioSource.PlayOneShot(fireClip);

            // �C���^�[�o���̍Đݒ�
            fireIntervalTimer = fireIntervalTime;
        }
    }
}
