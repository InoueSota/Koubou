using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // 基本情報
    private static Transition instance;

    // 遷移先
    private string nextSceneName;

    // フラグ類
    private bool isTransition;
    private bool isTransitionNow;

    [Header("自オブジェクト取得")]
    [SerializeField] private Camera secondaryCamera;

    [Header("黒幕")]
    [SerializeField] private SpriteRenderer blackSpriteRenderer;
    [SerializeField] private Ease blackEase;

    [Header("イージング")]
    [SerializeField] private float easeTime;

    void Awake()
    {
        // シーンが切り替わってもオブジェクトを保持するプログラム
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        isTransition = false;

        if (GameObject.FindWithTag("Transition"))
        {
            if (GameObject.FindWithTag("Transition") != gameObject)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnEnable()
    {
        // シーン読み込み
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        // シーン読み込み
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンがロードされた後の動作を再開
        if (isTransition)
        {
            DoTransition();
        }
    }

    void Update()
    {
        if (isTransition)
        {
            DoTransition();
        }
        if (Camera.main != null && Camera.main.GetUniversalAdditionalCameraData().cameraStack.Count == 0)
        {
            // カメラ追加処理
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(secondaryCamera);
        }
    }
    void DoTransition()
    {
        isTransitionNow = true;

        blackSpriteRenderer.DOColor(Color.black, easeTime).SetEase(blackEase).OnComplete(() =>
        {
            // ここで画面切り替わる処理(画面遷移で画面が見えてない場所)
            SceneManager.LoadScene(nextSceneName);
            blackSpriteRenderer.DOColor(new(0f, 0f, 0f, 0f), easeTime).SetEase(blackEase).OnComplete(() =>
            {
                // 画面遷移が閉じる
                isTransitionNow = false;
            });
        });

        isTransition = false;
    }

    // Setter
    public void SetTransition(string name)
    {
        isTransition = true;
        nextSceneName = name;
    }

    // Getter
    public bool GetIsTransitionNow()
    {
        return isTransitionNow;
    }
}
