using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    [Header("他オブジェクト取得")]
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;
    [SerializeField] private Transform groundTransform;

    // 基本情報
    private Vector3 halfSize;

    // 座標類
    private Vector3 targetPosition;

    [Header("Move")]
    [SerializeField] private float stalkerPower;
    [SerializeField] private float moveSpeed;
    private Vector3 inputVector;
    private Vector3 moveVector;
    private Vector3 saveVector;

    [Header("Dash")]
    [SerializeField] private float dashRange;
    [SerializeField] private float dashIntervalTime;
    private float dashIntervalTimer;
    private Vector3 dushVector;

    void Start()
    {
        inputManager = gameManagerObj.GetComponent<InputManager>();

        halfSize = transform.localScale * 0.5f;
        targetPosition = transform.position;
    }

    public void ManualUpdate()
    {
        // 入力情報を最新に更新する
        inputManager.GetAllInput();

        // 平面
        InputVector();
        Move();
        Dush();
        Look();

        StalkerPosition();
    }

    void InputVector()
    {
        inputVector.x = inputManager.ReturnInputValue(inputManager.horizontal);
        inputVector.z = inputManager.ReturnInputValue(inputManager.vertical);

        inputVector = Quaternion.Euler(0f, -20f, 0f) * inputVector;
    }
    void Move()
    {
        // 移動量を取得する
        moveVector.x = inputVector.x * moveSpeed;
        moveVector.z = inputVector.z * moveSpeed;

        // 移動方向を保存する
        if (inputManager.IsPush(inputManager.horizontal) || inputManager.IsPush(inputManager.vertical))
        {
            saveVector.x = inputVector.x;
            saveVector.z = inputVector.z;
        }

        // 移動量を加算する
        targetPosition += moveVector * Time.deltaTime;
    }
    void Look()
    {
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z) + saveVector);
    }
    void Dush()
    {
        dashIntervalTimer -= Time.deltaTime;

        if (inputManager.IsTrgger(inputManager.dash) && dashIntervalTimer <= 0f)
        {
            // 移動量を取得する
            dushVector.x = saveVector.x * dashRange;
            dushVector.z = saveVector.z * dashRange;

            // 移動量を加算する
            targetPosition += dushVector;

            // ダッシュが連続で行えないようインターバルを設定する
            dashIntervalTimer = dashIntervalTime;
        }
    }
    void StalkerPosition()
    {
        // 目標地点を目指す
        transform.position += (targetPosition - transform.position) * (stalkerPower * Time.deltaTime);
    }
}
