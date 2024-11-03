using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private Transform playerTransform;

    [Header("�ǐ�")]
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
