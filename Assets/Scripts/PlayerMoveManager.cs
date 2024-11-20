using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveManager : MonoBehaviour
{
    // Manager�擾
    private PlayerManager manager;

    // ��{���
    private Vector3 halfSize;

    // ���W��
    private Vector3 targetPosition;

    [Header("Move")]
    [SerializeField] private float stalkerPower;
    [SerializeField] private float normalStalkerPower;
    [SerializeField] private float normalSpeed;
    private float moveSpeed;
    private Vector3 inputVector;
    private Vector3 moveVector;
    private Vector3 saveVector;

    [Header("Dash")]
    [SerializeField] private float dashRange;
    [SerializeField] private float dashIntervalTime;
    private float dashIntervalTimer;
    private Vector3 dashVector;

    [Header("Run")]
    [SerializeField] private float runSpeed;
    private bool isRunning;

    [Header("Reaction")]
    [SerializeField] private float reactionStalkerPower;
    [SerializeField] private float reactionRange;
    [SerializeField] private float reactionTime;
    private float reactionTimer;

    [Header("UI")]
    [SerializeField] private Image dashGauge;

    [Header("Effects")]
    [SerializeField] private ParticleSystem runParticle;

    [Header("Other Object")]
    [SerializeField] private Transform bossCoreTransform;

    void Start()
    {
        manager = GetComponent<PlayerManager>();

        halfSize = transform.localScale * 0.5f;
        targetPosition = transform.position;
        saveVector = Vector3.forward;

        moveSpeed = normalSpeed;
        isRunning = false;
    }

    public void ManualUpdate()
    {
        if (manager.GetGameManager().GetIsGameActive())
        {
            // ����
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
        // �����ړ�����������
        if (isRunning && (!manager.GetInputManager().IsPush(manager.GetInputManager().dash) || 
            (!manager.GetInputManager().IsPush(manager.GetInputManager().horizontal) && !manager.GetInputManager().IsPush(manager.GetInputManager().vertical))) ||
            manager.GetInputManager().IsPush(manager.GetInputManager().lTrigger))
        {
            // ����G�t�F�N�g���A�N�e�B�u�ɂ���
            runParticle.Stop();

            moveSpeed = normalSpeed;
            isRunning = false;
        }

        // �ړ��ʂ��擾����
        moveVector.x = inputVector.x * moveSpeed;
        moveVector.z = inputVector.z * moveSpeed;

        // �ړ�������ۑ�����
        if (manager.GetInputManager().IsPush(manager.GetInputManager().horizontal) || manager.GetInputManager().IsPush(manager.GetInputManager().vertical))
        {
            saveVector.x = inputVector.x;
            saveVector.z = inputVector.z;
        }

        // �ړ��ʂ����Z����
        CheckAddPosition(moveVector.x * Time.deltaTime, true);
        CheckAddPosition(moveVector.z * Time.deltaTime, false);
    }
    void Look()
    {
        if (manager.GetInputManager().IsPush(manager.GetInputManager().lTrigger))
        {
            transform.LookAt(bossCoreTransform);
        }
        else
        {
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z) + saveVector);
        }
    }
    void Dash()
    {
        dashIntervalTimer -= Time.deltaTime;
        dashGauge.fillAmount = 1f - dashIntervalTimer / dashIntervalTime;

        if (manager.GetInputManager().IsTrgger(manager.GetInputManager().dash) && dashIntervalTimer <= 0f)
        {
            // ����t���O��true�ɂ���
            isRunning = true;

            // ����ړ����x�ɕύX����
            moveSpeed = runSpeed;

            // �ړ��ʂ��擾����
            dashVector.x = saveVector.x * dashRange;
            dashVector.z = saveVector.z * dashRange;

            // �ړ��ʂ����Z����
            targetPosition += dashVector;

            // �O�]����
            transform.DORotate(Vector3.right * 360f, 0.4f, RotateMode.LocalAxisAdd).OnComplete(CheckFinishRotate);

            // �_�b�V�����A���ōs���Ȃ��悤�C���^�[�o����ݒ肷��
            dashIntervalTimer = dashIntervalTime;
        }
    }
    void ClampInStage()
    {
        float subtractHalfSize = 25f - halfSize.x;

        // X��
        if (targetPosition.x > subtractHalfSize)
        {
            targetPosition.x = subtractHalfSize;
        }
        else if (targetPosition.x < -subtractHalfSize)
        {
            targetPosition.x = -subtractHalfSize;
        }

        // Z��
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
        float t = reactionTimer / reactionTime;
        stalkerPower = Mathf.Lerp(normalStalkerPower, reactionStalkerPower, 1 - (1 - t) * (1 - t));

        // �ڕW�n�_��ڎw��
        transform.position += (targetPosition - transform.position) * (stalkerPower * Time.deltaTime);

        // �^�C�}�[�̍X�V
        reactionTimer -= Time.deltaTime;
        reactionTimer = Mathf.Clamp(reactionTimer, 0f, 1f);
    }

    void CheckAddPosition(float _addValue, bool _isXaxis)
    {
        // X������
        if (_isXaxis)
        {
            targetPosition.x += _addValue;

            // Pillar
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                // X������
                float xBetween = Mathf.Abs(targetPosition.x - pillar.transform.position.x);
                float xDoubleSize = halfSize.x * 2f;

                // Z������
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
        // Z������
        else
        {
            targetPosition.z += _addValue;

            // Pillar
            foreach (GameObject pillar in GameObject.FindGameObjectsWithTag("Pillar"))
            {
                // X������
                float xBetween = Mathf.Abs(targetPosition.x - pillar.transform.position.x);
                float xDoubleSize = halfSize.x * 2f;

                // Z������
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
    void CheckFinishRotate()
    {
        if (isRunning)
        {
            // ����G�t�F�N�g���A�N�e�B�u�ɂ���
            runParticle.Play();
        }
    }

    // Setter
    public void Reaction(Vector3 _toPlayer)
    {
        // �����̐ݒ�
        targetPosition += _toPlayer * reactionRange;
        targetPosition.y = 0.5f;

        // Reaction���Ԃ̐ݒ�
        reactionTimer = reactionTime;

        // �X�e�[�W���Ɏ��߂�
        ClampInStage();
    }

    // Getter
    public Vector3 GetSaveVector()
    {
        return saveVector;
    }
}
