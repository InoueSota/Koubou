using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveManager : MonoBehaviour
{
    // Manager取得
    private PlayerManager manager;

    // 基本情報
    private Vector3 halfSize;

    // 座標類
    private Vector3 targetPosition;

    [Header("Move")]
    [SerializeField] private float stalkerPower;
    [SerializeField] private float normalSpeed;
    private float moveSpeed;
    private Vector3 inputVector;
    private Vector3 moveVector;
    private Vector3 saveVector;

    [Header("Dash")]
    [SerializeField] private float dashRange;
    [SerializeField] private float dashIntervalTime;
    private float dashIntervalTimer;
    private Vector3 dushVector;

    [Header("Run")]
    [SerializeField] private float runSpeed;
    private bool isRunning;

    [Header("UI")]
    [SerializeField] private Image dushGauge;

    void Start()
    {
        manager = GetComponent<PlayerManager>();

        halfSize = transform.localScale * 0.5f;
        targetPosition = transform.position;

        moveSpeed = normalSpeed;
        isRunning = false;
    }

    public void ManualUpdate()
    {
        if (manager.GetGameManager().GetIsGameActive())
        {
            // 平面
            InputVector();
            Move();
            Dash();
            Look();
            ClampInStage();

            StalkerPosition();
        }
    }

    void InputVector()
    {
        inputVector.x = manager.GetInputManager().ReturnInputValue(manager.GetInputManager().horizontal);
        inputVector.z = manager.GetInputManager().ReturnInputValue(manager.GetInputManager().vertical);

        inputVector = Quaternion.Euler(0f, -20f, 0f) * inputVector;
    }
    void Move()
    {
        // 高速移動を解消する
        if (isRunning && !manager.GetInputManager().IsPush(manager.GetInputManager().dash))
        {
            moveSpeed = normalSpeed;
            isRunning = false;
        }

        // 移動量を取得する
        moveVector.x = inputVector.x * moveSpeed;
        moveVector.z = inputVector.z * moveSpeed;

        // 移動方向を保存する
        if (manager.GetInputManager().IsPush(manager.GetInputManager().horizontal) || manager.GetInputManager().IsPush(manager.GetInputManager().vertical))
        {
            saveVector.x = inputVector.x;
            saveVector.z = inputVector.z;
        }

        // 移動量を加算する
        CheckAddPosition(moveVector.x * Time.deltaTime, true);
        CheckAddPosition(moveVector.z * Time.deltaTime, false);
    }
    void Look()
    {
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z) + saveVector);
    }
    void Dash()
    {
        dashIntervalTimer -= Time.deltaTime;
        dushGauge.fillAmount = 1f - dashIntervalTimer / dashIntervalTime;

        if (manager.GetInputManager().IsTrgger(manager.GetInputManager().dash) && dashIntervalTimer <= 0f)
        {
            // 走りフラグをtrueにする
            isRunning = true;

            // 走り移動速度に変更する
            moveSpeed = runSpeed;

            // 移動量を取得する
            dushVector.x = saveVector.x * dashRange;
            dushVector.z = saveVector.z * dashRange;

            // 移動量を加算する
            targetPosition += dushVector;

            // ダッシュが連続で行えないようインターバルを設定する
            dashIntervalTimer = dashIntervalTime;
        }
    }
    void ClampInStage()
    {
        float subtractHalfSize = 25f - halfSize.x;

        // X軸
        if (targetPosition.x > subtractHalfSize)
        {
            targetPosition.x = subtractHalfSize;
        }
        else if (targetPosition.x < -subtractHalfSize)
        {
            targetPosition.x = -subtractHalfSize;
        }

        // Z軸
        if (targetPosition.z > subtractHalfSize)
        {
            targetPosition.z = subtractHalfSize;
        }
        else if (targetPosition.z < -subtractHalfSize)
        {
            targetPosition.z = -subtractHalfSize;
        }
    }
    void StalkerPosition()
    {
        // 目標地点を目指す
        transform.position += (targetPosition - transform.position) * (stalkerPower * Time.deltaTime);
    }

    void CheckAddPosition(float _addValue, bool _isXaxis)
    {
        // X軸判定
        if (_isXaxis)
        {
            targetPosition.x += _addValue;

            // Pillar
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                // X軸判定
                float xBetween = Mathf.Abs(targetPosition.x - pillar.transform.position.x);
                float xDoubleSize = halfSize.x * 2f;

                // Z軸判定
                float zBetween = Mathf.Abs(targetPosition.z - pillar.transform.position.z);
                float zDoubleSize = halfSize.z * 2f;

                if (zBetween < zDoubleSize && xBetween < xDoubleSize)
                {
                    if (pillar.transform.position.x < targetPosition.x)
                    {
                        targetPosition.x = pillar.transform.position.x + 0.5f + halfSize.x;
                        break;
                    }
                    else
                    {
                        targetPosition.x = pillar.transform.position.x - 0.5f - halfSize.x;
                        break;
                    }
                }
            }
        }
        // Z軸判定
        else
        {
            targetPosition.z += _addValue;

            // Pillar
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                // X軸判定
                float xBetween = Mathf.Abs(targetPosition.x - pillar.transform.position.x);
                float xDoubleSize = halfSize.x * 2f;

                // Z軸判定
                float zBetween = Mathf.Abs(targetPosition.z - pillar.transform.position.z);
                float zDoubleSize = halfSize.z * 2f;

                if (zBetween < zDoubleSize && xBetween < xDoubleSize)
                {
                    if (pillar.transform.position.z < targetPosition.z)
                    {
                        targetPosition.z = pillar.transform.position.z + 0.5f + halfSize.z;
                        break;
                    }
                    else
                    {
                        targetPosition.z = pillar.transform.position.z - 0.5f - halfSize.z;
                        break;
                    }
                }
            }
        }
    }

    public void SetTargetPosition()
    {

        // ステージ内に収める
        ClampInStage();
    }
}
