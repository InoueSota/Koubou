using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;
    private MenuManager menuManager;

    [Header("Other Component")]
    [SerializeField] private Volume volume;
    private Bloom bloom;

    [Header("Particle Systems")]
    [SerializeField] private EffectSlashManager slashManager;

    [Header("Materials")]
    [SerializeField] private Material emissionPlayer;
    [SerializeField] private Material boss001;
    [SerializeField] private Material slash001;
    [SerializeField] private Material slash002;

    [Header("Uis")]
    [SerializeField] private Image bossName;
    [SerializeField] private Image bossHpGaugeFill;
    [SerializeField] private Image top;
    [SerializeField] private Image topGauge;
    [SerializeField] private Image bottom;
    [SerializeField] private Image bottomGauge;
    [SerializeField] private Image left;
    [SerializeField] private Image leftGauge;
    [SerializeField] private Image right;

    [Header("ClearDirection")]
    [SerializeField] private BossCoreManager bossCoreManager;
    [SerializeField] private float bloomTime;
    private float bloomTimer;
    [SerializeField] private float bloomTargetIntensity;
    private float bloomOriginIntensity;
    private float bloomOriginThreshold;
    private bool isGameClear;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        menuManager = GetComponent<MenuManager>();

        if (volume.profile.TryGet<Bloom>(out bloom)) { bloom.tint.Override(GlobalVariables.color1); }

        // Particle Systems
        slashManager.SetColor();

        // Materials
        var factor = Mathf.Pow(2, 2.5f);
        Color intensityColor1 = new(GlobalVariables.color1.r * factor, GlobalVariables.color1.g * factor, GlobalVariables.color1.b * factor);
        emissionPlayer.SetColor("_EmissionColor", intensityColor1);

        factor = Mathf.Pow(2, 3f);
        intensityColor1 = new(GlobalVariables.color1.r * factor, GlobalVariables.color1.g * factor, GlobalVariables.color1.b * factor);
        boss001.SetColor("_EmissionColor", intensityColor1);

        factor = Mathf.Pow(2, 4f);
        intensityColor1 = new(GlobalVariables.color1.r * factor, GlobalVariables.color1.g * factor, GlobalVariables.color1.b * factor);
        Color intensityColor2 = new(GlobalVariables.color2.r * factor, GlobalVariables.color2.g * factor, GlobalVariables.color2.b * factor);
        slash001.SetColor("_Color", intensityColor1);
        slash002.SetColor("_Color", intensityColor2);

        // Uis
        bossName.color = GlobalVariables.color1;
        bossHpGaugeFill.color = GlobalVariables.color1;
        top.color = GlobalVariables.color1;
        topGauge.color = GlobalVariables.color1;
        bottom.color = GlobalVariables.color1;
        bottomGauge.color = GlobalVariables.color1;
        left.color = GlobalVariables.color1;
        leftGauge.color = GlobalVariables.color1;
        right.color = GlobalVariables.color1;
    }
    void Update()
    {
        // 入力情報を最新に更新する
        inputManager.GetAllInput();

        Clear();
    }

    void Clear()
    {
        // クリアチェック
        if (!isGameClear && bossCoreManager.GetBossHp() <= 0f)
        {
            // Bloom演出初期化
            bloomTimer = bloomTime;
            bloomOriginIntensity = bloom.intensity.value;

            isGameClear = true;
        }

        if (isGameClear)
        {
            // 経過時間計測
            bloomTimer -= Time.deltaTime;
            bloomTimer = Mathf.Clamp(bloomTimer, 0f, bloomTime);

            float t = bloomTimer / bloomTime;

            // Color
            Color tmpColor = Color.Lerp(Color.white, GlobalVariables.color1, t * t);
            if (volume.profile.TryGet<Bloom>(out bloom)) { bloom.tint.Override(tmpColor); }

            // Threshold
            bloom.threshold.value = Mathf.Lerp(0f, 1f, t * t);

            // Intensity
            bloom.intensity.value = Mathf.Lerp(bloomTargetIntensity, bloomOriginIntensity, t * t);
        }
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }

    public bool GetIsGameActive()
    {
        if (menuManager.GetIsActive())
        {
            return false;
        }
        return true;
    }
}
