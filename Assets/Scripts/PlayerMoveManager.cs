using DG.Tweening;
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
    private bool isDashing;

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

    [Header("Sounds")]
    [SerializeField] private AudioClip dashClip;
    private AudioSource audioSource;

    void Start()
    {
        manager = GetComponent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();

        halfSize = transform.localScale * 0.5f;
        targetPosition = transform.position;
        saveVector = Vector3.forward;

        moveSpeed = normalSpeed;

        // フラグ類
        isDashing = false;
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
        if (isRunning && (!manager.GetInputManager().IsPush(manager.GetInputManager().dash) || 
            (!manager.GetInputManager().IsPush(manager.GetInputManager().horizontal) && !manager.GetInputManager().IsPush(manager.GetInputManager().vertical))) ||
            manager.GetInputManager().IsPush(manager.GetInputManager().lTrigger))
        {
            // 走りエフェクトを非アクティブにする
            runParticle.Stop();

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

            // 正規化
            saveVector = Vector3.Normalize(saveVector);
        }

        // 移動量を加算する
        CheckAddPosition(moveVector.x * Time.deltaTime, true);
        CheckAddPosition(moveVector.z * Time.deltaTime, false);
    }
    void Look()
    {
        // 前転中は処理を通さない
        if (!isDashing)
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
    }
    void Dash()
    {
        dashIntervalTimer -= Time.deltaTime;
        dashGauge.fillAmount = 1f - dashIntervalTimer / dashIntervalTime;

        if (manager.GetInputManager().IsTrgger(manager.GetInputManager().dash) && dashIntervalTimer <= 0f)
        {
            // 前転する
            transform.DORotate(Vector3.right * 360f, 0.4f, RotateMode.LocalAxisAdd).OnComplete(CheckFinishRotate);

            // 前転フラグをtrueにする
            isDashing = true;

            // 走りフラグをtrueにする
            isRunning = true;

            // 走り移動速度に変更する
            moveSpeed = runSpeed;

            // 移動量を取得する
            dashVector.x = saveVector.x * dashRange;
            dashVector.z = saveVector.z * dashRange;

            // 移動量を加算する
            targetPosition += dashVector;

            // 音を鳴らす
            audioSource.PlayOneShot(dashClip);

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
        float t = reactionTimer / reactionTime;
        stalkerPower = Mathf.Lerp(normalStalkerPower, reactionStalkerPower, 1 - (1 - t) * (1 - t));

        // 目標地点を目指す
        transform.position += (targetPosition - transform.position) * (stalkerPower * Time.deltaTime);

        // タイマーの更新
        reactionTimer -= Time.deltaTime;
        reactionTimer = Mathf.Clamp(reactionTimer, 0f, 1f);
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
                if (CheckHitObject(pillar.transform.position, true))
                {
                    break;
                }
            }

            // Light
            foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light"))
            {
                if (CheckHitObject(light.transform.position, true))
                {
                    break;
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
                if (CheckHitObject(pillar.transform.position, false))
                {
                    break;
                }
            }

            // Light
            foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light"))
            {
                if (CheckHitObject(light.transform.position, false))
                {
                    break;
                }
            }
        }
    }
    bool CheckHitObject(Vector3 _otherPosition, bool _isXaxis)
    {
        // X軸判定
        float xBetween = Mathf.Abs(targetPosition.x - _otherPosition.x);
        float xDoubleSize = halfSize.x * 2f;

        // Z軸判定
        float zBetween = Mathf.Abs(targetPosition.z - _otherPosition.z);
        float zDoubleSize = halfSize.z * 2f;

        if (zBetween < zDoubleSize && xBetween < xDoubleSize)
        {
            if (_isXaxis)
            {
                if (_otherPosition.x < targetPosition.x)
                {
                    targetPosition.x = _otherPosition.x + 0.5f + halfSize.x;
                    return true;
                }
                else
                {
                    targetPosition.x = _otherPosition.x - 0.5f - halfSize.x;
                    return true;
                }
            }
            else
            {
                if (_otherPosition.z < targetPosition.z)
                {
                    targetPosition.z = _otherPosition.z + 0.5f + halfSize.z;
                    return true;
                }
                else
                {
                    targetPosition.z = _otherPosition.z - 0.5f - halfSize.z;
                    return true;
                }
            }
        }
        return false;
    }
    void CheckFinishRotate()
    {
        // 前転フラグをfalseにする
        isDashing = false;

        if (isRunning)
        {
            // 走りエフェクトをアクティブにする
            runParticle.Play();
        }
    }

    // Setter
    public void Reaction(Vector3 _toPlayer)
    {
        // 反動の設定
        targetPosition += _toPlayer * reactionRange;
        targetPosition.y = 0.5f;

        // Reaction時間の設定
        reactionTimer = reactionTime;

        // ステージ内に収める
        ClampInStage();
    }

    // Getter
    public Vector3 GetSaveVector()
    {
        return saveVector;
    }
}
