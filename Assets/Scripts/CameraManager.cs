using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("他オブジェクト取得")]
    [SerializeField] private Transform playerTransform;

    [Header("追跡")]
    [SerializeField] private float chasePower;
    private Vector3 saveDistance;

    void Start()
    {
        saveDistance = transform.position - playerTransform.position;
    }

    void Update()
    {
        Chase();
    }
    void Chase()
    {
        Vector3 targetPosition = playerTransform.position + saveDistance;
        transform.position += (targetPosition - transform.position) * (chasePower * Time.deltaTime);
    }
}
