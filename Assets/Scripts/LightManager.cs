using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private GameManager gameManager;

    [Header("Light")]
    [SerializeField] private GameObject lightObj;
    [SerializeField] private float lightTime;
    private float lightTimer;
    private bool isLighting;

    [Header("Gauge")]
    [SerializeField] private Image gauge;
    [SerializeField] private float gaugeTime;
    private float gaugeTimer;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        lightObj.SetActive(false);
        isLighting = false;
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
            gaugeTimer += Time.deltaTime;

            gauge.fillAmount = gaugeTimer / gaugeTime;

            // �^�C�}�[�̍Đݒ�
            if (gaugeTimer >= gaugeTime)
            {
                lightTimer = lightTime;
                lightObj.SetActive(true);
                isLighting = true;
            }
        }
        else
        {
            lightTimer -= Time.deltaTime;

            gauge.fillAmount = lightTimer / lightTime;

            // �^�C�}�[�̍Đݒ�
            if (lightTimer <= 0f)
            {
                gaugeTimer = 0f;
                lightObj.SetActive(false);
                isLighting = false;
            }
        }
    }
}
