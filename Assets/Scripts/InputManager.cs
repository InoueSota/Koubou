using UnityEngine;

public class InputManager : MonoBehaviour
{
    // ��{�N���X
    public class InputPattern
    {
        public float input = 0f;
        public float preInput = 0f;

        private bool isGetInput = false;

        public void GetInput(string _inputName)
        {
            if (!isGetInput)
            {
                preInput = input;
                input = Input.GetAxisRaw(_inputName);

                isGetInput = true;
            }
        }
        public void SetIsGetInput(bool _isGetInput)
        {
            isGetInput = _isGetInput;
        }
    }

    // ���͂̎��
    public InputPattern horizontal;
    public InputPattern vertical;
    public InputPattern attack;
    public InputPattern cancel;
    public InputPattern decide;
    public InputPattern dash;
    public InputPattern menu;

    void Start()
    {
        horizontal = new InputPattern();
        vertical = new InputPattern();
        attack = new InputPattern();
        cancel = new InputPattern();
        decide = new InputPattern();
        dash = new InputPattern();
        menu = new InputPattern();
    }

    public void SetIsGetInput()
    {
        horizontal.SetIsGetInput(false);
        vertical.SetIsGetInput(false);
        attack.SetIsGetInput(false);
        cancel.SetIsGetInput(false);
        decide.SetIsGetInput(false);
        dash.SetIsGetInput(false);
        menu.SetIsGetInput(false);
    }

    public void GetAllInput()
    {
        horizontal.GetInput("Horizontal");
        vertical.GetInput("Vertical");
        attack.GetInput("Attack");
        cancel.GetInput("Cancel");
        decide.GetInput("Decide");
        dash.GetInput("Dash");
        menu.GetInput("Menu");
    }

    public bool IsTrgger(InputPattern _inputPattern)
    {
        if (_inputPattern.input != 0f && _inputPattern.preInput == 0f)
        {
            return true;
        }
        return false;
    }

    public bool IsPush(InputPattern _inputPattern)
    {
        if (_inputPattern.input != 0f)
        {
            return true;
        }
        return false;
    }

    public bool IsRelease(InputPattern _inputPattern)
    {
        if (_inputPattern.input == 0f && _inputPattern.preInput != 0f)
        {
            return true;
        }
        return false;
    }

    public float ReturnInputValue(InputPattern _inputPattern)
    {
        return _inputPattern.input;
    }
}