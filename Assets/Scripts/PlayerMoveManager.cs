using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;
    [SerializeField] private Transform groundTransform;

    // ��{���
    private Vector3 halfSize;

    // ���W��
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
        // ���͏����ŐV�ɍX�V����
        inputManager.GetAllInput();

        // ����
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
        // �ړ��ʂ��擾����
        moveVector.x = inputVector.x * moveSpeed;
        moveVector.z = inputVector.z * moveSpeed;

        // �ړ�������ۑ�����
        if (inputManager.IsPush(inputManager.horizontal) || inputManager.IsPush(inputManager.vertical))
        {
            saveVector.x = inputVector.x;
            saveVector.z = inputVector.z;
        }

        // �ړ��ʂ����Z����
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
            // �ړ��ʂ��擾����
            dushVector.x = saveVector.x * dashRange;
            dushVector.z = saveVector.z * dashRange;

            // �ړ��ʂ����Z����
            targetPosition += dushVector;

            // �_�b�V�����A���ōs���Ȃ��悤�C���^�[�o����ݒ肷��
            dashIntervalTimer = dashIntervalTime;
        }
    }
    void StalkerPosition()
    {
        // �ڕW�n�_��ڎw��
        transform.position += (targetPosition - transform.position) * (stalkerPower * Time.deltaTime);
    }
}
