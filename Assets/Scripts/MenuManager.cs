using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // 自コンポーネント取得
    private InputManager inputManager;

    // 他コンポーネント取得
    private Transition transition;

    // フラグ
    private bool isActive;

    enum Contents
    {
        RETURN,
        TOTITLE
    }
    private Contents contents = Contents.RETURN;

    [Header("Color")]
    private Color selectColor;
    [SerializeField] private Color unSelectColor;

    [Header("Position")]
    [SerializeField] private float selectX;
    [SerializeField] private float unSelectX;
    [SerializeField] private float selectTabX;
    [SerializeField] private float unSelectTabX;
    [SerializeField] private float chasePower;

    [Header("UI")]
    // Black
    [SerializeField] private Image blackImage;
    private Color blackTargetColor;
    // Tab
    [SerializeField] private Transform tabTransform;
    private Vector3 tabTargetPosition;
    // Return
    [SerializeField] private Transform returnTransform;
    private Vector3 returnTargetPosition;
    [SerializeField] private Image returnImage;
    private Color returnTargetColor;
    // ToTitle
    [SerializeField] private Transform toTitleTransform;
    private Vector3 toTitleTargetPosition;
    [SerializeField] private Image toTitleImage;
    private Color toTitleTargetColor;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

        if (GameObject.FindWithTag("Transition"))
        {
            transition = GameObject.FindWithTag("Transition").GetComponent<Transition>();
        }

        isActive = false;

        // Color
        selectColor = GlobalVariables.color1;
        blackTargetColor = blackImage.color;
        returnTargetColor = selectColor;
        toTitleTargetColor = unSelectColor;

        // Target
        tabTargetPosition = new(unSelectTabX, tabTransform.localPosition.y, tabTransform.localPosition.z);
        tabTransform.localPosition = tabTargetPosition;
        returnTargetPosition = new(selectX, returnTransform.localPosition.y, returnTransform.localPosition.z);
        returnTransform.localPosition = returnTargetPosition;
        toTitleTargetPosition = new(unSelectX, toTitleTransform.localPosition.y, toTitleTransform.localPosition.z);
        toTitleTransform.localPosition = toTitleTargetPosition;
    }

    void Update()
    {
        // 入力情報を最新に更新する
        inputManager.GetAllInput();

        if (isActive)
        {
            SelectContents();

            if (inputManager.IsTrgger(inputManager.menu))
            {
                tabTargetPosition.x = unSelectTabX;
                blackTargetColor = new(0f, 0f, 0f, 0f);
                ToReturn();
                contents = Contents.RETURN;
                isActive = false;
            }
        }
        else if (inputManager.IsTrgger(inputManager.menu))
        {
            tabTargetPosition.x = selectTabX;
            blackTargetColor = new(0f, 0f, 0f, 0.6f);
            isActive = true;
        }

        StalkerTarget();
    }
    void SelectContents()
    {
        switch (contents)
        {
            case Contents.RETURN:

                if (inputManager.IsTrgger(inputManager.decide))
                {
                    tabTargetPosition.x = unSelectTabX;
                    blackTargetColor = new(0f, 0f, 0f, 0f);
                    isActive = false;
                }

                if (inputManager.IsTrgger(inputManager.vertical) && inputManager.ReturnInputValue(inputManager.vertical) < 0f)
                {
                    ToToTitle();
                    contents = Contents.TOTITLE;
                }

                break;
            case Contents.TOTITLE:

                // タイトルに遷移する
                if (inputManager.IsTrgger(inputManager.decide) && !transition.GetIsTransitionNow())
                {
                    transition.SetTransition("TitleScene");
                }

                if (inputManager.IsTrgger(inputManager.vertical) && inputManager.ReturnInputValue(inputManager.vertical) > 0f)
                {
                    ToReturn();
                    contents = Contents.RETURN;
                }

                break;
        }
    }
    void ToReturn()
    {
        returnTargetPosition.x = selectX;
        returnTargetColor = selectColor;
        toTitleTargetPosition.x = unSelectX;
        toTitleTargetColor = unSelectColor;
    }
    void ToToTitle()
    {
        returnTargetPosition.x = unSelectX;
        returnTargetColor = unSelectColor;
        toTitleTargetPosition.x = selectX;
        toTitleTargetColor = selectColor;
    }
    void StalkerTarget()
    {
        float deltaChasePower = chasePower * Time.deltaTime;

        // Color
        blackImage.color += (blackTargetColor - blackImage.color) * deltaChasePower;
        returnImage.color += (returnTargetColor - returnImage.color) * deltaChasePower;
        toTitleImage.color += (toTitleTargetColor - toTitleImage.color) * deltaChasePower;

        // Position
        tabTransform.localPosition += (tabTargetPosition - tabTransform.localPosition) * deltaChasePower;
        returnTransform.localPosition += (returnTargetPosition - returnTransform.localPosition) * deltaChasePower;
        toTitleTransform.localPosition += (toTitleTargetPosition - toTitleTransform.localPosition) * deltaChasePower;
    }

    // Setter
    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }

    // Getter
    public bool GetIsActive()
    {
        return isActive;
    }
}
