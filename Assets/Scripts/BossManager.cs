using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Child Objects")]
    [SerializeField] private Transform ignoreRotate;
    [SerializeField] private Transform cubeParent;

    [Header("Other Objects")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform pillarParent;

    // MoveType
    private enum MoveType
    {
        IDLE,
        GETCLOSE,
        METEORITE
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

    void Start()
    {
        moveType = MoveType.IDLE;

        // Idle
        idleTimer = idleTime;
    }

    void Update()
    {
        // �R�A��]
        CoreRotate();

        switch (moveType)
        {
            case MoveType.IDLE:

                TypeIdle();

                // �ҋ@�I��
                if (idleTimer <= 0f) { ChangeFromIdle(); }

                break;
            case MoveType.GETCLOSE:

                TypeGetClose();

                // �ړ��I��
                if (judgeCount >= 4) { ChangeMoveType(MoveType.IDLE); }

                break;
            case MoveType.METEORITE:

                TypeMeteorite();

                // �ҋ@�I��
                if (afterMeteoriteTimer <= 0f)
                {
                    // �L���[�u��߂�
                    cubeParent.gameObject.SetActive(true);

                    ChangeMoveType(MoveType.IDLE);
                }

                break;
        }
    }
    void ChangeMoveType(MoveType _moveType)
    {
        // �J�ڐ��MoveType�̏��������s��
        switch (_moveType)
        {
            case MoveType.IDLE:

                // �C���^�[�o���̏�����
                idleTimer = idleTime;

                break;
            case MoveType.GETCLOSE:

                // �x�N�g���̏�����
                toGetClosePlayerVector = Vector3.zero;

                // �J�E���g�̏�����
                judgeCount = 0;

                // �C���^�[�o���̏�����
                judgeIntervalTimer = judgeIntervalTime;

                break;
            case MoveType.METEORITE:

                // �C���^�[�o���̏�����
                meteoriteIdleTimer = meteoriteIdleTime;
                meteoriteIntervalTimer = meteoriteIntervalTime;
                afterMeteoriteTimer = afterMeteoriteTime;

                // �t���O�̏�����
                isFinishMeteoriteIdle = false;
                isFinishMeteoriteCreate = false;

                break;
        }

        // �J�ڂ���
        moveType = _moveType;
    }
    void ChangeFromIdle()
    {
        // �����_���̐������擾����
        int randomNumber = Random.Range(0, 99);

        if (randomNumber % 4 == 0)
        { ChangeMoveType(MoveType.METEORITE); }
        else
        { ChangeMoveType(MoveType.GETCLOSE); }
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
        // ��]
        CubeRotate(idleRotateAddValue);

        // �C���^�[�o���̌v��
        idleTimer -= Time.deltaTime;
    }
    void TypeGetClose()
    {
        // �L���[�u��]
        CubeRotate(getCloseRotateAddValue);

        // �C���^�[�o���̌v��
        judgeIntervalTimer -= Time.deltaTime;

        // �ړ�
        transform.position += toGetClosePlayerVector * Time.deltaTime;

        // �ړ������̍Đݒ�
        if (judgeIntervalTimer <= 0f)
        {
            // �v���C���[�̍��W�𓖃I�u�W�F�N�g�̍����ɒ�������
            Vector3 adjustPlayerPosition = new(playerTransform.position.x, transform.position.y, playerTransform.position.z);

            // �v���C���[�̂�������擾���A�ړ��ʂ��|����
            toGetClosePlayerVector = Vector3.Normalize(adjustPlayerPosition - transform.position) * getCloseSpeed;

            // �J�E���g�̉��Z
            judgeCount++;

            // �C���^�[�o���̍Đݒ�
            judgeIntervalTimer = judgeIntervalTime;
        }
    }
    void TypeMeteorite()
    {
        // 覐΍s���܂ł̑ҋ@���Ԃ��v��
        meteoriteIdleTimer -= Time.deltaTime;

        // �ҋ@�I��
        if (!isFinishMeteoriteIdle && meteoriteIdleTimer <= 0f)
        {
            // �L���[�u������
            cubeParent.gameObject.SetActive(false);

            // �p�[�e�B�N�����o��

            // �ҋ@�I���t���O��true�ɂ���
            isFinishMeteoriteIdle = true;
        }

        // �ҋ@���I����������
        if (isFinishMeteoriteIdle)
        {
            // 覐΂𗎂Ƃ��܂ł̑ҋ@���Ԃ��v��
            meteoriteIntervalTimer -= Time.deltaTime;

            // �ҋ@�I��
            if (!isFinishMeteoriteCreate && meteoriteIntervalTimer <= 0f)
            {
                // �T��������
                for (int i = 0; i < 5; i++)
                {
                    // �P�̓v���C���[�̐^��ɗ��Ƃ�
                    if (i == 0)
                    {
                        // ������\��
                        GameObject fallInfomation = Instantiate(fallInfomationPrefab, new(playerTransform.position.x, 0f, playerTransform.position.z), Quaternion.identity);

                        // ����
                        GameObject meteorite = Instantiate(meteoritePrefab, transform.position, Quaternion.identity);

                        // ������
                        meteorite.GetComponent<MeteoriteManager>().Initialize(playerTransform.position, fallInfomation, pillarParent, playerTransform.position);
                    }
                    // �c��̓����_���Ȉʒu�i���͈͓��j�ɗ��Ƃ�
                    else
                    {
                        // �����_���ȍ��W���i�[����Vector3
                        Vector3 randomPosition = Vector3.zero;
                        randomPosition.x = Random.Range(-25f, 25f);
                        randomPosition.y = playerTransform.position.y;
                        randomPosition.z = Random.Range(-25f, 25f);

                        // �v���C���[�����苗�����ɗ��Ƃ���܂Ń����_�����v�Z��������
                        while (Vector3.Distance(randomPosition, playerTransform.position) > meteoriteRange)
                        {
                            randomPosition.x = Random.Range(-25f, 25f);
                            randomPosition.z = Random.Range(-25f, 25f);
                        }

                        // ������\��
                        GameObject fallInfomation = Instantiate(fallInfomationPrefab, new(randomPosition.x, 0f, randomPosition.z), Quaternion.identity);

                        // ����
                        GameObject meteorite = Instantiate(meteoritePrefab, transform.position, Quaternion.identity);

                        // ������
                        meteorite.GetComponent<MeteoriteManager>().Initialize(randomPosition, fallInfomation, pillarParent, playerTransform.position);
                    }
                }

                // �����I���t���O��true�ɂ���
                isFinishMeteoriteCreate = true;
            }

            if (isFinishMeteoriteCreate)
            {
                // 覐΂𗎉���������̑ҋ@���Ԃ��v��
                afterMeteoriteTimer -= Time.deltaTime;
            }
        }
    }
    void CubeRotate(float _rotateAddValue)
    {
        // ��]�ʉ��Z
        rotateTargetValue += _rotateAddValue * Time.deltaTime;

        // �ǐ�
        rotateValue += (rotateTargetValue - rotateValue) * (rotateChasePower * Time.deltaTime);

        // 360���𒴂�����360�������
        if (rotateValue >= 360f)
        {
            rotateTargetValue -= 360f;
            rotateValue -= 360f;
        }

        // ��]
        cubeParent.localRotation = Quaternion.Euler(0f, rotateValue, 0f);
    }
}
