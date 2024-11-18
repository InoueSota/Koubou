using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private InputManager inputManager;

    // ���R���|�[�l���g�擾
    [SerializeField] private Volume volume;
    private Bloom bloom;
    private Transition transition;
    private ColorData colorData;

    enum Contents
    {
        NEWGAME,
        QUIT
    }
    private Contents contents = Contents.NEWGAME;

    [Header("Color")]
    private Color selectColor;
    [SerializeField] private Color unSelectColor;
    private Color bloomColor;

    [Header("Position")]
    [SerializeField] private float selectX;
    [SerializeField] private float unSelectX;
    [SerializeField] private float chasePower;

    [Header("UI")]
    // NewGame
    [SerializeField] private GameObject newGame;
    private Vector3 newGameTargetPosition;
    [SerializeField] private Image newGameImage;
    private Color newGameTargetColor;
    // Quit
    [SerializeField] private GameObject quit;
    private Vector3 quitTargetPosition;
    [SerializeField] private Image quitImage;
    private Color quitTargetColor;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

        if (GameObject.FindWithTag("Transition"))
        {
            transition = GameObject.FindWithTag("Transition").GetComponent<Transition>();
        }

        // �F�f�[�^�擾
        colorData = new ColorData();
        colorData.Initialize();
        ColorInitialize();
        // Color
        bloomColor = selectColor;
        BloomChangeColor();
        newGameTargetColor = selectColor;
        quitTargetColor = unSelectColor;

        // Target
        newGameTargetPosition = new(selectX, newGame.transform.localPosition.y, newGame.transform.localPosition.z);
        newGame.transform.localPosition = newGameTargetPosition;
        quitTargetPosition = quit.transform.localPosition;

        // Camera
        if (transition)
        {
            var cameraData = mainCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(transition.transform.GetChild(0).GetComponent<Camera>());
        }
    }

    void Update()
    {
        // ���͏����ŐV�ɍX�V����
        inputManager.GetAllInput();

        SelectContents();
        ChangeColor();

        StalkerTarget();
    }
    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }
    void SelectContents()
    {
        switch (contents)
        {
            case Contents.NEWGAME:

                // �C���Q�[���ɑJ�ڂ���
                if (inputManager.IsTrgger(inputManager.decide) && !transition.GetIsTransitionNow())
                {
                    transition.SetTransition("GameScene");
                }

                if (inputManager.IsTrgger(inputManager.vertical) && inputManager.ReturnInputValue(inputManager.vertical) < 0f)
                {
                    ToQuit();
                    contents = Contents.QUIT;
                }

                break;
            case Contents.QUIT:

                // �E�B���h�E�����
                if (inputManager.IsTrgger(inputManager.decide))
                {
                    Application.Quit();
                }

                if (inputManager.IsTrgger(inputManager.vertical) && inputManager.ReturnInputValue(inputManager.vertical) > 0f)
                {
                    ToNewGame();
                    contents = Contents.NEWGAME;
                }

                break;
        }
    }
    void ToNewGame()
    {
        newGameTargetPosition.x = selectX;
        newGameTargetColor = selectColor;
        quitTargetPosition.x = unSelectX;
        quitTargetColor = unSelectColor;
    }
    void ToQuit()
    {
        newGameTargetPosition.x = unSelectX;
        newGameTargetColor = unSelectColor;
        quitTargetPosition.x = selectX;
        quitTargetColor = selectColor;
    }
    void StalkerTarget()
    {
        float deltaChasePower = chasePower * Time.deltaTime;

        // Color
        bloomColor += (selectColor - bloomColor) * deltaChasePower;
        BloomChangeColor();
        newGameImage.color += (newGameTargetColor - newGameImage.color) * deltaChasePower;
        quitImage.color += (quitTargetColor - quitImage.color) * deltaChasePower;

        // Position
        newGame.transform.localPosition += (newGameTargetPosition - newGame.transform.localPosition) * deltaChasePower;
        quit.transform.localPosition += (quitTargetPosition - quit.transform.localPosition) * deltaChasePower;
    }
    void ChangeColor()
    {
        if (inputManager.IsTrgger(inputManager.dPad))
        {
            if (inputManager.ReturnInputValue(inputManager.dPad) < 0f)
            {
                if (GlobalVariables.colorNum > 0)
                {
                    GlobalVariables.colorNum--;
                }
                else
                {
                    GlobalVariables.colorNum = colorData.maxColorNum - 1;
                }
            }
            else if (inputManager.ReturnInputValue(inputManager.dPad) > 0f)
            {
                if (GlobalVariables.colorNum < colorData.maxColorNum - 1)
                {
                    GlobalVariables.colorNum++;
                }
                else
                {
                    GlobalVariables.colorNum = 0;
                }
            }
            ColorInitialize();
        }
    }
    void ColorInitialize()
    {
        // �F���
        GlobalVariables.color1 = colorData.GetMainColor(GlobalVariables.colorNum);
        GlobalVariables.color2 = colorData.GetSubColor(GlobalVariables.colorNum);
        selectColor = GlobalVariables.color1;

        switch (contents)
        {
            case Contents.NEWGAME:
                ToNewGame();
                break;
            case Contents.QUIT:
                ToQuit();
                break;
        }
    }
    void BloomChangeColor()
    {
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.tint.Override(bloomColor);
        }
    }
}
