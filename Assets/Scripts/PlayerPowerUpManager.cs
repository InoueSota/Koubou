using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour
{
    // MyComponent
    private PlayerManager manager;
    private MeshRenderer meshRenderer;

    // �t���O��
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

    void Start()
    {
        // GetComponent
        manager = GetComponent<PlayerManager>();
        meshRenderer = GetComponent<MeshRenderer>();

        // �t���O�ޏ�����
        isPowerUp = false;
        isPowerUpFrame = false;

        // �}�e���A��������
        meshRenderer.material = normalMat;
    }

    public void ManualUpdate()
    {
        // PowerUp�����u�Ԃ̃t���[���t���O��false�ɂ���
        isPowerUpFrame = false;

        Light();
        PowerUp();
    }

    void Light()
    {
        // �����󂯎��邩��������
        if (manager.GetInputManager().IsTrgger(manager.GetInputManager().decide))
        {
            foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light"))
            {
                LightManager lightManager = light.GetComponent<LightManager>();

                // �����肪���Ă��邩
                if (lightManager.GetIsLightning())
                {
                    Vector3 lightRePosition = light.transform.position;

                    if (IsHitObject(ref lightRePosition))
                    {
                        // PowerUp�����u�Ԃ̃t���[���t���O��true�ɂ���
                        isPowerUpFrame = true;

                        // Light������
                        lightManager.SetDark();

                        // Material�ύX
                        meshRenderer.material = powerUpMat;

                        // �C���^�[�o���̐ݒ�
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
            // �C���^�[�o���̍X�V
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                // Material�ύX
                meshRenderer.material = normalMat;

                // �t���O������
                isPowerUp = false;
            }
        }
    }

    bool IsHitObject(ref Vector3 _objectPosition)
    {
        // ������Player�ɍ��킹���V���W
        _objectPosition = new(_objectPosition.x, transform.position.y, _objectPosition.z);

        // �������擾
        float distance = Vector3.Distance(transform.position, _objectPosition);

        // LightRange�������肷��
        if (distance < lightRange)
        {
            return true;
        }
        return false;
    }

    // Getter
    public bool GetIsPowerUpFrame()
    {
        return isPowerUpFrame;
    }
    public float GetLightRange()
    {
        return lightRange;
    }
}
