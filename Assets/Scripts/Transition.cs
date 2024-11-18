using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // ��{���
    private static Transition instance;

    // �J�ڐ�
    private string nextSceneName;

    // �t���O��
    private bool isTransition;
    private bool isTransitionNow;

    [Header("���I�u�W�F�N�g�擾")]
    [SerializeField] private Camera secondaryCamera;

    [Header("����")]
    [SerializeField] private SpriteRenderer blackSpriteRenderer;
    [SerializeField] private Ease blackEase;

    [Header("�C�[�W���O")]
    [SerializeField] private float easeTime;

    void Awake()
    {
        // �V�[�����؂�ւ���Ă��I�u�W�F�N�g��ێ�����v���O����
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
        // �V�[���ǂݍ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        // �V�[���ǂݍ���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�[�������[�h���ꂽ��̓�����ĊJ
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
            // �J�����ǉ�����
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(secondaryCamera);
        }
    }
    void DoTransition()
    {
        isTransitionNow = true;

        blackSpriteRenderer.DOColor(Color.black, easeTime).SetEase(blackEase).OnComplete(() =>
        {
            // �����ŉ�ʐ؂�ւ�鏈��(��ʑJ�ڂŉ�ʂ������ĂȂ��ꏊ)
            SceneManager.LoadScene(nextSceneName);
            blackSpriteRenderer.DOColor(new(0f, 0f, 0f, 0f), easeTime).SetEase(blackEase).OnComplete(() =>
            {
                // ��ʑJ�ڂ�����
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
