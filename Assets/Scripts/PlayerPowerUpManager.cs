using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour
{
    // MyComponent
    private PlayerManager manager;
    private MeshRenderer meshRenderer;

    // フラグ類
    private bool isPowerUp;
    private bool isPowerUpFrame;

    [Header("PowerUp")]
    [SerializeField] private float powerUpTime;
    private float powerUpTimer;

    [Header("Light")]
    [SerializeField] private float lightRange;

    [Header("Materials")]
    [SerializeField] private Material normalMat;
    [SerializeField] private Material powerUpMat;

    [Header("Sounds")]
    [SerializeField] private AudioClip powerUpClip;
    private AudioSource audioSource;

    void Start()
    {
        // GetComponent
        manager = GetComponent<PlayerManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        // フラグ類初期化
        isPowerUp = false;
        isPowerUpFrame = false;

        // マテリアル初期化
        meshRenderer.material = normalMat;
    }

    public void ManualUpdate()
    {
        // PowerUpした瞬間のフレームフラグをfalseにする
        isPowerUpFrame = false;

        Light();
        PowerUp();
    }

    void Light()
    {
        // 光を受け取れるか判定を取る
        if (manager.GetInputManager().IsTrgger(manager.GetInputManager().attack))
        {
            foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light"))
            {
                LightManager lightManager = light.GetComponent<LightManager>();

                // 明かりがついているか
                if (lightManager.GetIsLightning())
                {
                    Vector3 lightRePosition = light.transform.position;

                    if (IsHitObject(ref lightRePosition))
                    {
                        // PowerUpした瞬間のフレームフラグをtrueにする
                        isPowerUpFrame = true;

                        // Lightを消す
                        lightManager.SetDark();

                        // Material変更
                        meshRenderer.material = powerUpMat;

                        // 音を鳴らす
                        audioSource.PlayOneShot(powerUpClip);

                        // インターバルの設定
                        powerUpTimer = powerUpTime;
                        isPowerUp = true;
                    }
                }
            }
        }
    }
    void PowerUp()
    {
        if (isPowerUp)
        {
            // インターバルの更新
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                // Material変更
                meshRenderer.material = normalMat;

                // フラグ初期化
                isPowerUp = false;
            }
        }
    }

    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // 高さをPlayerに合わせた新座標
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // 距離を取得
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // LightRange内か判定する
        if (distance < lightRange)
        {
            return true;
        }
        return false;
    }

    // Getter
    public bool GetIsPowerUp()
    {
        return isPowerUp;
    }
    public bool GetIsPowerUpFrame()
    {
        return isPowerUpFrame;
    }
    public float GetLightRange()
    {
        return lightRange;
    }
}
