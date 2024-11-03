using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 自コンポーネント取得
    private PlayerMoveManager moveManager;

    void Start()
    {
        moveManager = GetComponent<PlayerMoveManager>();
    }

    void Update()
    {
        moveManager.ManualUpdate();
    }
}
