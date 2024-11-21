using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    // Manager�擾
    private PlayerManager manager;
    private PlayerMoveManager moveManager;

    [Header("Slash")]
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

    void Start()
    {
        manager = GetComponent<PlayerManager>();
        moveManager = GetComponent<PlayerMoveManager>();
    }

    public void ManualUpdate()
    {
        Slash();
        Fire();
    }
    void Slash()
    {
        // �U���̃C���^�[�o���v��
        attackIntervalTimer -= Time.deltaTime;
        attackGauge.fillAmount = 1f - attackIntervalTimer / attackIntervalTime;

        // �U�����s��
        if (attackIntervalTimer <= 0f && manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            // Slash�̐���
            GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);

            // �ϐ���^����
            slash.GetComponent<PlayerSlashManager>().Initialize(moveManager, bossCoreTransform, adjustDistance);

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
            bullet.GetComponent<PlayerBulletManager>().Initialize(bossCoreTransform, moveVector, adjustDistance);

            // �C���^�[�o���̍Đݒ�
            fireIntervalTimer = fireIntervalTime;
        }
    }
}
