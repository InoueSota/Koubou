using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    void Update()
    {
        // P�L�[�Ń^�C�g��
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("TitleScene");
        }

        // �E�B���h�E�����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
