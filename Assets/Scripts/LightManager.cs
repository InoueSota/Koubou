using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    // 他コンポーネント取得
    private PlayerPowerUpManager powerUpManager;
    private GameManager gameManager;

    [Header("Light")]
    [SerializeField] private GameObject lightObj;
    private bool isLighting;

    [Header("Gauge")]
    [SerializeField] private Image gauge;
    [SerializeField] private float gaugeMag;
    [SerializeField] private float gaugeTime;
    private float gaugeTimer;

    [Header("Abutton")]
    [SerializeField] private Image aButtonImage;
    [SerializeField] private float aButtonChasePower;
    private Color aButtonTargetColor;

    void Start()
    {
        powerUpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerUpManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        lightObj.SetActive(false);
        isLighting = false;

        aButtonTargetColor = new(1f, 1f, 1f, 0f);
        aButtonImage.color = aButtonTargetColor;
    }

    void Update()
    {
        if (gameManager.GetIsGameActive())
        {
            LightGauge();
        }
    }
    void LightGauge()
    {
        if (!isLighting)
        {
            // Playerの座標を調整
            Vector3 playerRePosition = powerUpManager.transform.position;

            if (IsHitObject(ref playerRePosition, powerUpManager.GetLightRange()))
            {
                gaugeTimer += Time.deltaTime * gaugeMag;
            }
            else
            {
                gaugeTimer += Time.deltaTime;
            }
            gauge.fillAmount = gaugeTimer / gaugeTime;

            // タイマーの再設定
            if (gaugeTimer >= gaugeTime)
            {
                lightObj.SetActive(true);
                isLighting = true;
            }
        }
        else
        {
            // Playerの座標を調整
            Vector3 playerRePosition = powerUpManager.transform.position;

            // AButtonの表示
            if (IsHitObject(ref playerRePosition, powerUpManager.GetLightRange()))
            {
                aButtonTargetColor = Color.white;
            }
            else
            {
                aButtonTargetColor = new(1f, 1f, 1f, 0f);
            }

            // 目標色に向かって色を変更する
            aButtonImage.color += (aButtonTargetColor - aButtonImage.color) * (aButtonChasePower * Time.deltaTime);
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition, float _range)
    {
        // 高さをPlayerに合わせた新座標
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // 距離を取得
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // Range内か判定する
        if (distance < _range)
        {
            return true;
        }
        return false;
    }

    // Getter
    public bool GetIsLightning()
    {
        return isLighting;
    }

    // Setter
    public void SetDark()
    {
        // ゲージ初期化
        gaugeTimer = 0f;

        // 暗くする
        lightObj.SetActive(false);

        // フラグ初期化
        isLighting = false;

        // Aボタンの色を透明にする
        aButtonTargetColor = new(1f, 1f, 1f, 0f);
        aButtonImage.color = aButtonTargetColor;
    }
}
