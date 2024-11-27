using UnityEngine;

public class BossCubeManager : MonoBehaviour
{
    [Header("My Object")]
    [SerializeField] private Transform coreTransform;

    [Header("Other Object")]
    [SerializeField] private GameObject playerObj;
    private PlayerHpManager playerHpManager;

    [Header("Parameter")]
    [SerializeField] private float damageRange;

    // Position
    private Vector3 originPosition;

    // Vector
    private Vector3 diffDirection;

    // Process
    enum MgrProcess
    {
        NONE,
        SPREAD,
        FIRE,
        IDLE,
        RETURN
    }
    private MgrProcess mgrProcess = MgrProcess.NONE;

    [Header("Spread")]
    [SerializeField] private float spreadTime;
    private float spreadTimer;
    [SerializeField] private float angleTarget;
    private float angle;
    [SerializeField] private float diffTarget;
    private float diff;

    [Header("Fire")]
    [SerializeField] private float fireTime;
    private float fireTimer;
    [SerializeField] private float addAnglePower;
    [SerializeField] private float fireIntervalTime;
    private float fireIntervalTimer;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Idle")]
    [SerializeField] private float idleTime;
    private float idleTimer;

    [Header("Return")]
    [SerializeField] private float returnTime;
    private float returnTimer;
    private Vector3 returnStartPosition;
    private Quaternion returnStartQuaternion;

    void Start()
    {
        playerHpManager = playerObj.GetComponent<PlayerHpManager>();

        originPosition = transform.localPosition;

        diffDirection = Vector3.Normalize(transform.localPosition);
    }

    void Update()
    {
        Attack();
        MerryGoRound();
    }
    void Attack()
    {
        // �v���C���[�̍��W�𒲐�����
        Vector3 playerRePosition = new(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);

        // ��苗�����Ƀv���C���[��������_���[�W��^����
        if (Vector3.Distance(playerRePosition, transform.position) <= damageRange)
        {
            playerHpManager.Damage();
        }
    }
    void MerryGoRound()
    {
        switch (mgrProcess)
        {
            case MgrProcess.SPREAD:

                // �ړ����Ԃ̌v��
                spreadTimer -= Time.deltaTime;
                spreadTimer = Mathf.Clamp(spreadTimer, 0f, spreadTime);

                // ��]�ړ��̏�����
                transform.localPosition = originPosition;
                transform.localRotation = Quaternion.identity;

                // �ڕW��ڎw��
                float t = spreadTimer / spreadTime;
                angle = Mathf.Lerp(angleTarget, 0f, t);
                diff  = Mathf.Lerp( diffTarget, 0f, t);

                // ��]�ړ�
                transform.localPosition += diffDirection * diff;
                transform.RotateAround(coreTransform.position, Vector3.up, angle);

                // �ړ��I��
                if (spreadTimer <= 0f) { mgrProcess = MgrProcess.FIRE; }

                break;
            case MgrProcess.FIRE:

                // �U���C���^�[�o���̌v��
                fireIntervalTimer -= Time.deltaTime;

                // �e����
                if (fireIntervalTimer <= 0f)
                {
                    // �e����
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.GetComponent<BossBulletManager>().Initialize(coreTransform.position, playerHpManager);

                    // �U���C���^�[�o���̍Đݒ�
                    fireIntervalTimer = fireIntervalTime;
                }

                // �U�����Ԃ̌v��
                fireTimer -= Time.deltaTime;
                fireTimer = Mathf.Clamp(fireTimer, 0f, fireTime);

                // ��]�ړ��̏�����
                transform.localPosition = originPosition;
                transform.localRotation = Quaternion.identity;

                // ��]����
                angle += addAnglePower * Time.deltaTime;

                // ��]�ړ�
                transform.localPosition += diffDirection * diff;
                transform.RotateAround(coreTransform.position, Vector3.up, angle);

                // �U���I��
                if (fireTimer <= 0f)
                {
                    // ���W�̎擾
                    returnStartPosition = transform.localPosition;

                    // ��]�ʂ̎擾
                    returnStartQuaternion = transform.localRotation;

                    mgrProcess = MgrProcess.IDLE;
                }

                break;
            case MgrProcess.IDLE:

                // �ҋ@���Ԃ̌v��
                idleTimer -= Time.deltaTime;
                idleTimer = Mathf.Clamp(idleTimer, 0f, idleTime);

                // �ҋ@�I��
                if (idleTimer <= 0f) { mgrProcess = MgrProcess.RETURN; }

                break;
            case MgrProcess.RETURN:

                // �ړ����Ԃ̌v��
                returnTimer -= Time.deltaTime;
                returnTimer = Mathf.Clamp(returnTimer, 0f, returnTime);

                // �ڕW��ڎw��
                float returnT = returnTimer / returnTime;
                transform.localPosition = Vector3.Lerp(originPosition, returnStartPosition, returnT);
                transform.localRotation = Quaternion.Lerp(Quaternion.identity, returnStartQuaternion, returnT);

                // �ړ��I��
                if (returnTimer <= 0f) { mgrProcess = MgrProcess.NONE; }

                break;
        }
    }

    // Setter
    public void SetMgrStart()
    {
        // �C���^�[�o���̏�����
        spreadTimer = spreadTime;
        fireTimer = fireTime;
        idleTimer = idleTime;
        returnTimer = returnTime;

        // �p�����[�^�[�̏�����
        angle = 0f;
        diff = 0f;

        // Process�J�n
        mgrProcess = MgrProcess.SPREAD;
    }
}
