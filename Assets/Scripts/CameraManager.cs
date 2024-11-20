using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("他オブジェクト取得")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;
    [SerializeField] private Transform bossTransform;

    [Header("追跡")]
    [SerializeField] private float chasePower;
    private Vector3 saveDistance;

    [Header("狙える最大距離")]
    [SerializeField] private float maxDistance;

    void Start()
    {
        inputManager = gameManagerObj.GetComponent<InputManager>();

        saveDistance = transform.position - playerTransform.position;
    }

    void Update()
    {
        // 入力情報を最新に更新する
        inputManager.GetAllInput();

        Chase();
    }
    void Chase()
    {
        // プレイヤーから一定距離離したベクトルを算出
        Vector3 targetPosition = playerTransform.position + saveDistance;

        // 一定距離よりも近い && Ltriggerを押しているとき
        if (Vector3.Distance(bossTransform.position, playerTransform.position) <= maxDistance && inputManager.IsPush(inputManager.lTrigger))
        {
            // プレイヤーからボスに向かったベクトルを算出
            Vector3 toBoss = bossTransform.position - playerTransform.position;
            // プレイヤーとボスの中間地点に調整する
            targetPosition += toBoss * 0.5f;
        }

        transform.position += (targetPosition - transform.position) * (chasePower * Time.deltaTime);
    }
}
