using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private InputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        // ���͏����ŐV�ɍX�V����
        inputManager.GetAllInput();
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }
}
