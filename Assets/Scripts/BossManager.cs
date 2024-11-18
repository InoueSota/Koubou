using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Child Objects")]
    [SerializeField] private Transform ignoreRotate;
    [SerializeField] private Transform cubeParent;

    // MoveType
    private enum MoveType
    {
        IDLE,
    }
    private MoveType moveType = MoveType.IDLE;

    [Header("Core Rotate")]
    [SerializeField] private float coreRotateChasePower;
    [SerializeField] private float coreRotateAddValue;
    private float coreRotateTargetValue;
    private float coreRotateValue;

    [Header("Idle")]
    [SerializeField] private float idleRotateChasePower;
    [SerializeField] private float idleRotateAddValue;
    private float idleRotateTargetValue;
    private float idleRotateValue;

    void Start()
    {
        
    }

    void Update()
    {
        CoreRotate();

        switch (moveType)
        {
            case MoveType.IDLE:

                TypeIdle();

                break;
        }
    }
    void CoreRotate()
    {
        // ��]�ʉ��Z
        coreRotateTargetValue += coreRotateAddValue * Time.deltaTime;

        // �ǐ�
        coreRotateValue += (coreRotateTargetValue - coreRotateValue) * (coreRotateChasePower * Time.deltaTime);

        // 360���𒴂�����360�������
        if (coreRotateValue >= 360f)
        {
            coreRotateTargetValue -= 360f;
            coreRotateValue -= 360f;
        }

        // ��]
        transform.localRotation = Quaternion.Euler(0f, coreRotateValue, 0f);

        // ��]�ʂ𖳎�
        ignoreRotate.localRotation = Quaternion.Euler(0f, -coreRotateValue, 0f);
    }
    void TypeIdle()
    {
        // ��]�ʉ��Z
        idleRotateTargetValue += idleRotateAddValue * Time.deltaTime;

        // �ǐ�
        idleRotateValue += (idleRotateTargetValue - idleRotateValue) * (idleRotateChasePower * Time.deltaTime);

        // 360���𒴂�����360�������
        if (idleRotateValue >= 360f)
        {
            idleRotateTargetValue -= 360f;
            idleRotateValue -= 360f;
        }

        // ��]
        cubeParent.localRotation = Quaternion.Euler(0f, idleRotateValue, 0f);
    }
}
