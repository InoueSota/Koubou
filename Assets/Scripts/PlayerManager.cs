using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
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
