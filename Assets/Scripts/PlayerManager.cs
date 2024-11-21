using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerMoveManager moveManager;
    private PlayerAttackManager attackManager;
    private PlayerPowerUpManager powerUpManager;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;
    private GameManager gameManager;

    void Start()
    {
        moveManager = GetComponent<PlayerMoveManager>();
        attackManager = GetComponent<PlayerAttackManager>();
        powerUpManager = GetComponent<PlayerPowerUpManager>();

        inputManager = gameManagerObj.GetComponent<InputManager>();
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    void Update()
    {
        // ���͏����ŐV�ɍX�V����
        inputManager.GetAllInput();

        powerUpManager.ManualUpdate();
        moveManager.ManualUpdate(powerUpManager.GetIsPowerUpFrame());
        attackManager.ManualUpdate();
    }

    // Getter
    public InputManager GetInputManager()
    {
        return inputManager;
    }
    public GameManager GetGameManager()
    {
        return gameManager;
    }
}
