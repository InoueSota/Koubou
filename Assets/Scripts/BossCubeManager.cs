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
        // プレイヤーの座標を調整する
        Vector3 playerRePosition = new(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);

        // 一定距離内にプレイヤーがいたらダメージを与える
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

                // 移動時間の計測
                spreadTimer -= Time.deltaTime;
                spreadTimer = Mathf.Clamp(spreadTimer, 0f, spreadTime);

                // 回転移動の初期化
                transform.localPosition = originPosition;
                transform.localRotation = Quaternion.identity;

                // 目標を目指す
                float t = spreadTimer / spreadTime;
                angle = Mathf.Lerp(angleTarget, 0f, t);
                diff  = Mathf.Lerp( diffTarget, 0f, t);

                // 回転移動
                transform.localPosition += diffDirection * diff;
                transform.RotateAround(coreTransform.position, Vector3.up, angle);

                // 移動終了
                if (spreadTimer <= 0f) { mgrProcess = MgrProcess.FIRE; }

                break;
            case MgrProcess.FIRE:

                // 攻撃インターバルの計測
                fireIntervalTimer -= Time.deltaTime;

                // 弾発射
                if (fireIntervalTimer <= 0f)
                {
                    // 弾生成
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.GetComponent<BossBulletManager>().Initialize(coreTransform.position, playerHpManager);

                    // 攻撃インターバルの再設定
                    fireIntervalTimer = fireIntervalTime;
                }

                // 攻撃時間の計測
                fireTimer -= Time.deltaTime;
                fireTimer = Mathf.Clamp(fireTimer, 0f, fireTime);

                // 回転移動の初期化
                transform.localPosition = originPosition;
                transform.localRotation = Quaternion.identity;

                // 回転する
                angle += addAnglePower * Time.deltaTime;

                // 回転移動
                transform.localPosition += diffDirection * diff;
                transform.RotateAround(coreTransform.position, Vector3.up, angle);

                // 攻撃終了
                if (fireTimer <= 0f)
                {
                    // 座標の取得
                    returnStartPosition = transform.localPosition;

                    // 回転量の取得
                    returnStartQuaternion = transform.localRotation;

                    mgrProcess = MgrProcess.IDLE;
                }

                break;
            case MgrProcess.IDLE:

                // 待機時間の計測
                idleTimer -= Time.deltaTime;
                idleTimer = Mathf.Clamp(idleTimer, 0f, idleTime);

                // 待機終了
                if (idleTimer <= 0f) { mgrProcess = MgrProcess.RETURN; }

                break;
            case MgrProcess.RETURN:

                // 移動時間の計測
                returnTimer -= Time.deltaTime;
                returnTimer = Mathf.Clamp(returnTimer, 0f, returnTime);

                // 目標を目指す
                float returnT = returnTimer / returnTime;
                transform.localPosition = Vector3.Lerp(originPosition, returnStartPosition, returnT);
                transform.localRotation = Quaternion.Lerp(Quaternion.identity, returnStartQuaternion, returnT);

                // 移動終了
                if (returnTimer <= 0f) { mgrProcess = MgrProcess.NONE; }

                break;
        }
    }

    // Setter
    public void SetMgrStart()
    {
        // インターバルの初期化
        spreadTimer = spreadTime;
        fireTimer = fireTime;
        idleTimer = idleTime;
        returnTimer = returnTime;

        // パラメーターの初期化
        angle = 0f;
        diff = 0f;

        // Process開始
        mgrProcess = MgrProcess.SPREAD;
    }
}
