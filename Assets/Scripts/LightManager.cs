using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerPowerUpManager powerUpManager;
    private GameManager gameManager;

    [Header("Light")]
    [SerializeField] private GameObject lightObj;
    private bool isLighting;

    [Header("Gauge")]
    [SerializeField] private Image gauge;
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
            gaugeTimer += Time.deltaTime;

            gauge.fillAmount = gaugeTimer / gaugeTime;

            // �^�C�}�[�̍Đݒ�
            if (gaugeTimer >= gaugeTime)
            {
                lightObj.SetActive(true);
                isLighting = true;
            }
        }
        else
        {
            // Player�̍��W�𒲐�
            Vector3 playerRePosition = powerUpManager.transform.position;

            if (IsHitObject(ref playerRePosition))
            {
                aButtonTargetColor = Color.white;
            }
            else
            {
                aButtonTargetColor = new(1f, 1f, 1f, 0f);
            }

            // �ڕW�F�Ɍ������ĐF��ύX����
            aButtonImage.color += (aButtonTargetColor - aButtonImage.color) * (aButtonChasePower * Time.deltaTime);
        }
    }
    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // ������Player�ɍ��킹���V���W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // �������擾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // LightRange�������肷��
        if (distance < powerUpManager.GetLightRange())
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
        // �Q�[�W������
        gaugeTimer = 0f;

        // �Â�����
        lightObj.SetActive(false);

        // �t���O������
        isLighting = false;

        // A�{�^���̐F�𓧖��ɂ���
        aButtonTargetColor = new(1f, 1f, 1f, 0f);
        aButtonImage.color = aButtonTargetColor;
    }
}
