using UnityEngine;

public class BossManager : MonoBehaviour
{
    // My Component
    private BossCoreManager bossCoreManager;

    [Header("Child Objects")]
    [SerializeField] private Transform ignoreRotate;
    [SerializeField] private Transform cubeParent;
    [SerializeField] private BossCubeManager[] cubeManager;

    [Header("Other Objects")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform pillarParent;

    // MoveType
    private enum MoveType
    {
        IDLE,
        GETCLOSE,
        METEORITE,
        MERRYGOROUND
    }
    private MoveType moveType = MoveType.IDLE;

    [Header("Core Rotate")]
    [SerializeField] private float coreRotateChasePower;
    [SerializeField] private float coreRotateAddValue;
    private float coreRotateTargetValue;
    private float coreRotateValue;

    [Header("Cube Rotate")]
    [SerializeField] private float rotateChasePower;
    private float rotateTargetValue;
    private float rotateValue;

    [Header("Idle")]
    [SerializeField] private float idleRotateAddValue;
    [SerializeField] private float idleTime;
    private float idleTimer;

    [Header("Get Close")]
    [SerializeField] private float getCloseRotateAddValue;
    [SerializeField] private float getCloseSpeed;
    [SerializeField] private float judgeIntervalTime;
    private float judgeIntervalTimer;
    private Vector3 toGetClosePlayerVector;
    private int judgeCount;

    [Header("Meteorite")]
    [SerializeField] private GameObject meteoritePrefab;
    [SerializeField] private GameObject fallInfomationPrefab;
    [SerializeField] private float meteoriteRange;
    [SerializeField] private float meteoriteIdleTime;
    private float meteoriteIdleTimer;
    [SerializeField] private float meteoriteIntervalTime;
    private float meteoriteIntervalTimer;
    [SerializeField] private float afterMeteoriteTime;
    private float afterMeteoriteTimer;
    private bool isFinishMeteoriteIdle;
    private bool isFinishMeteoriteCreate;

    [Header("MerryGoRound")]
    [SerializeField] private float mgrIdleTime;
    private float mgrIdleTimer;
    [SerializeField] private float mgrTime;
    private float mgrTimer;
    private bool isFinishMgrIdle;

    void Start()
    {
        // ダメージを受けないようにする
        bossCoreManager = GetComponent<BossCoreManager>();
        bossCoreManager.SetCanHit(false);

        // Idle
        idleTimer = idleTime;
    }

    void Update()
    {
        // コア回転
        CoreRotate();

        switch (moveType)
        {
            case MoveType.IDLE:

                TypeIdle();

                // 待機終了
                if (idleTimer <= 0f) { ChangeFromIdle(); }

                break;
            case MoveType.GETCLOSE:

                TypeGetClose();

                // 移動終了
                if (judgeCount >= 4) { ChangeMoveType(MoveType.IDLE); }

                break;
            case MoveType.METEORITE:

                TypeMeteorite();

                // 待機終了
                if (afterMeteoriteTimer <= 0f)
                {
                    // キューブを戻す
                    cubeParent.gameObject.SetActive(true);

                    // ダメージを受けないようにする
                    bossCoreManager.SetCanHit(false);

                    ChangeMoveType(MoveType.IDLE);
                }

                break;
            case MoveType.MERRYGOROUND:

                TypeMerryGoRound();

                // 待機終了
                if (mgrTimer <= 0f)
                {
                    // ダメージを受けないようにする
                    bossCoreManager.SetCanHit(false);

                    ChangeMoveType(MoveType.IDLE);
                }

                break;
        }
    }
    void ChangeMoveType(MoveType _moveType)
    {
        // 遷移先のMoveTypeの初期化を行う
        switch (_moveType)
        {
            case MoveType.IDLE:

                // インターバルの初期化
                idleTimer = idleTime;

                // ダメージを受けないようにする
                bossCoreManager.SetCanHit(false);

                break;
            case MoveType.GETCLOSE:

                // ベクトルの初期化
                toGetClosePlayerVector = Vector3.zero;

                // カウントの初期化
                judgeCount = 0;

                // インターバルの初期化
                judgeIntervalTimer = judgeIntervalTime;

                break;
            case MoveType.METEORITE:

                // インターバルの初期化
                meteoriteIdleTimer = meteoriteIdleTime;
                meteoriteIntervalTimer = meteoriteIntervalTime;
                afterMeteoriteTimer = afterMeteoriteTime;

                // フラグの初期化
                isFinishMeteoriteIdle = false;
                isFinishMeteoriteCreate = false;

                break;
            case MoveType.MERRYGOROUND:

                // インターバルの初期化
                mgrIdleTimer = mgrIdleTime;
                mgrTimer = mgrTime;

                // フラグの初期化
                isFinishMgrIdle = false;

                break;
        }

        // 遷移する
        moveType = _moveType;
    }
    void ChangeFromIdle()
    {
        ChangeMoveType(MoveType.METEORITE);

        // ランダムの数字を取得する
        //int randomNumber = Random.Range(0, 99);

        //if (randomNumber % 4 == 0)
        //{ ChangeMoveType(MoveType.METEORITE); }
        //else
        //{ ChangeMoveType(MoveType.GETCLOSE); }
    }
    void CoreRotate()
    {
        // 回転量加算
        coreRotateTargetValue += coreRotateAddValue * Time.deltaTime;

        // 追跡
        coreRotateValue += (coreRotateTargetValue - coreRotateValue) * (coreRotateChasePower * Time.deltaTime);

        // 360°を超えたら360°分削る
        if (coreRotateValue >= 360f)
        {
            coreRotateTargetValue -= 360f;
            coreRotateValue -= 360f;
        }

        // 回転
        transform.localRotation = Quaternion.Euler(0f, coreRotateValue, 0f);

        // 回転量を無視
        ignoreRotate.localRotation = Quaternion.Euler(0f, -coreRotateValue, 0f);
    }
    void TypeIdle()
    {
        // 回転
        CubeRotate(idleRotateAddValue);

        // インターバルの計測
        idleTimer -= Time.deltaTime;
    }
    void TypeGetClose()
    {
        // キューブ回転
        CubeRotate(getCloseRotateAddValue);

        // インターバルの計測
        judgeIntervalTimer -= Time.deltaTime;

        // 移動
        transform.position += toGetClosePlayerVector * Time.deltaTime;

        // 移動方向の再設定
        if (judgeIntervalTimer <= 0f)
        {
            // プレイヤーの座標を当オブジェクトの高さに調整する
            Vector3 adjustPlayerPosition = new(playerTransform.position.x, transform.position.y, playerTransform.position.z);

            // プレイヤーのいる方を取得し、移動量を掛ける
            toGetClosePlayerVector = Vector3.Normalize(adjustPlayerPosition - transform.position) * getCloseSpeed;

            // カウントの加算
            judgeCount++;

            // インターバルの再設定
            judgeIntervalTimer = judgeIntervalTime;
        }
    }
    void TypeMeteorite()
    {
        // 隕石行動までの待機時間を計測
        meteoriteIdleTimer -= Time.deltaTime;

        // 待機終了
        if (!isFinishMeteoriteIdle && meteoriteIdleTimer <= 0f)
        {
            // ダメージを受けるようにする
            bossCoreManager.SetCanHit(true);

            // キューブを消す
            cubeParent.gameObject.SetActive(false);

            // パーティクルを出す

            // 待機終了フラグをtrueにする
            isFinishMeteoriteIdle = true;
        }

        // 待機が終了したあと
        if (isFinishMeteoriteIdle)
        {
            // 隕石を落とすまでの待機時間を計測
            meteoriteIntervalTimer -= Time.deltaTime;

            // 待機終了
            if (!isFinishMeteoriteCreate && meteoriteIntervalTimer <= 0f)
            {
                // ５個生成する
                for (int i = 0; i < 5; i++)
                {
                    // １つはプレイヤーの真上に落とす
                    if (i == 0)
                    {
                        // 落下先表示
                        GameObject fallInfomation = Instantiate(fallInfomationPrefab, new(playerTransform.position.x, 0f, playerTransform.position.z), Quaternion.identity);

                        // 生成
                        GameObject meteorite = Instantiate(meteoritePrefab, transform.position, Quaternion.identity);

                        // 初期化
                        meteorite.GetComponent<MeteoriteManager>().Initialize(playerTransform.position, fallInfomation, pillarParent, playerTransform.position);
                    }
                    // 残りはランダムな位置（一定範囲内）に落とす
                    else
                    {
                        // ランダムな座標を格納するVector3
                        Vector3 randomPosition = Vector3.zero;
                        randomPosition.x = Random.Range(-25f, 25f);
                        randomPosition.y = playerTransform.position.y;
                        randomPosition.z = Random.Range(-25f, 25f);

                        // プレイヤーから一定距離内に落とせるまでランダムを計算し続ける
                        while (Vector3.Distance(randomPosition, playerTransform.position) > meteoriteRange)
                        {
                            randomPosition.x = Random.Range(-25f, 25f);
                            randomPosition.z = Random.Range(-25f, 25f);
                        }

                        // 落下先表示
                        GameObject fallInfomation = Instantiate(fallInfomationPrefab, new(randomPosition.x, 0f, randomPosition.z), Quaternion.identity);

                        // 生成
                        GameObject meteorite = Instantiate(meteoritePrefab, transform.position, Quaternion.identity);

                        // 初期化
                        meteorite.GetComponent<MeteoriteManager>().Initialize(randomPosition, fallInfomation, pillarParent, playerTransform.position);
                    }
                }

                // 生成終了フラグをtrueにする
                isFinishMeteoriteCreate = true;
            }

            if (isFinishMeteoriteCreate)
            {
                // 隕石を落下させた後の待機時間を計測
                afterMeteoriteTimer -= Time.deltaTime;
            }
        }
    }
    void TypeMerryGoRound()
    {
        // 隕石行動までの待機時間を計測
        mgrIdleTimer -= Time.deltaTime;

        // 待機終了
        if (!isFinishMgrIdle && mgrIdleTimer <= 0f)
        {
            // キューブに開始指示を出す
            for (int i = 0; i < cubeManager.Length; i++)
            {
                if (i != 4)
                {
                    cubeManager[i].SetMgrStart();
                }
            }

            // ダメージを受けるようにする
            bossCoreManager.SetCanHit(true);

            // 待機終了フラグをtrueにする
            isFinishMgrIdle = true;
        }

        // 待機が終了したあと
        if (isFinishMgrIdle)
        {
            // mgr攻撃をしている時間を計測
            mgrTimer -= Time.deltaTime;
        }
    }
    void CubeRotate(float _rotateAddValue)
    {
        // 回転量加算
        rotateTargetValue += _rotateAddValue * Time.deltaTime;

        // 追跡
        rotateValue += (rotateTargetValue - rotateValue) * (rotateChasePower * Time.deltaTime);

        // 360°を超えたら360°分削る
        if (rotateValue >= 360f)
        {
            rotateTargetValue -= 360f;
            rotateValue -= 360f;
        }

        // 回転
        cubeParent.localRotation = Quaternion.Euler(0f, rotateValue, 0f);
    }
}
