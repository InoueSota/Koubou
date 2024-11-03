using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 自コンポーネント取得
    private InputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        // 入力情報を最新に更新する
        inputManager.GetAllInput();
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }
}
