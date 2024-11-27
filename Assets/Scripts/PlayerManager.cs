using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerMoveManager moveManager;
    private PlayerAttackManager attackManager;
    private PlayerPowerUpManager powerUpManager;

    [Header("他オブジェクト取得")]
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
        // 入力情報を最新に更新する
        inputManager.GetAllInput();

        powerUpManager.ManualUpdate();
        moveManager.ManualUpdate();
        attackManager.ManualUpdate(powerUpManager.GetIsPowerUpFrame());
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
