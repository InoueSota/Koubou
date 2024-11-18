using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private PlayerMoveManager moveManager;
    private PlayerAttackManager attackManager;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;
    private GameManager gameManager;

    void Start()
    {
        moveManager = GetComponent<PlayerMoveManager>();
        attackManager = GetComponent<PlayerAttackManager>();

        inputManager = gameManagerObj.GetComponent<InputManager>();
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    void Update()
    {
        // ���͏����ŐV�ɍX�V����
        inputManager.GetAllInput();

        moveManager.ManualUpdate();
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
