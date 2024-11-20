using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;
    [SerializeField] private Transform bossTransform;

    [Header("�ǐ�")]
    [SerializeField] private float chasePower;
    private Vector3 saveDistance;

    [Header("�_����ő勗��")]
    [SerializeField] private float maxDistance;

    void Start()
    {
        inputManager = gameManagerObj.GetComponent<InputManager>();

        saveDistance = transform.position - playerTransform.position;
    }

    void Update()
    {
        // ���͏����ŐV�ɍX�V����
        inputManager.GetAllInput();

        Chase();
    }
    void Chase()
    {
        // �v���C���[�����苗���������x�N�g�����Z�o
        Vector3 targetPosition = playerTransform.position + saveDistance;

        // ��苗�������߂� && Ltrigger�������Ă���Ƃ�
        if (Vector3.Distance(bossTransform.position, playerTransform.position) <= maxDistance && inputManager.IsPush(inputManager.lTrigger))
        {
            // �v���C���[����{�X�Ɍ��������x�N�g�����Z�o
            Vector3 toBoss = bossTransform.position - playerTransform.position;
            // �v���C���[�ƃ{�X�̒��Ԓn�_�ɒ�������
            targetPosition += toBoss * 0.5f;
        }

        transform.position += (targetPosition - transform.position) * (chasePower * Time.deltaTime);
    }
}
