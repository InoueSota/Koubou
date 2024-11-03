using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    void Update()
    {
        // Pキーでタイトル
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("TitleScene");
        }

        // ウィンドウを閉じる
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
