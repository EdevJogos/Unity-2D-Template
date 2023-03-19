using System.Collections;
using UnityEngine;

public class Display : MonoBehaviour
{
    public enum AnimationStyles
    {
        DIRECT,
        ANIMATOR,
        TWEEN,
    }

    public static System.Action<Displays, int> onActionRequested;
    public static System.Action<Displays, int, object> onDataActionRequested;

    public DisplayTypes type;
    public Displays ID;
    public AnimationStyles animationStyle;
    public bool receiveInputWhenActive;
    [SerializeField] protected UIOption _startSelected;

    protected UIOption _curSelected;

    public virtual void Initiate() { }
    public virtual void Initialize() { }

    public virtual void Show(bool p_show, System.Action p_callback, float p_ratio)
    {
        if (p_show)
        {
            StartInputListeners();

            if (_startSelected != null)
            {
                _startSelected.Select();
                _curSelected = _startSelected;
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }
        else
        {
            StopInputListeners();
        }

        switch (animationStyle)
        {
            case AnimationStyles.DIRECT:
                GetComponent<Canvas>().enabled = p_show;
                break;
            case AnimationStyles.ANIMATOR:
                HandleAnimator(p_show, p_callback, p_ratio);
                break;
            case AnimationStyles.TWEEN:
                break;
        }
    }

    private void HandleAnimator(bool p_show, System.Action p_callback, float p_ratio)
    {
        string __animation = p_show ? "In" : "Out";

        if (TryGetComponent(out Animator __animator))
        {
            __animator.SetTrigger(__animation);

            if (p_callback != null) StartCoroutine(RoutineShow(__animation, p_callback, p_ratio));
        }
        else
        {
            Debug.LogError("Requested animator animation style but animator could not be found.");
        }
    }

    private IEnumerator RoutineShow(string p_animation, System.Action p_callback, float p_ratio)
    {
        float __delay = GetClipLength(p_animation) * GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).speed * p_ratio;
        
        yield return new WaitForSecondsRealtime(__delay);

        p_callback?.Invoke();
    }

    private float GetClipLength(string name)
    {
        AnimationClip[] clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (var item in clips)
        {
            if (item.name == name)
            {
                return item.length;
            }
        }

        return 0;
    }

    #region Inputs
    protected virtual void StartInputListeners() => InputManager.InputListeners.ForEach(listener => StartInputListener(listener));
    protected virtual void StopInputListeners() => InputManager.InputListeners.ForEach(listener => StopInputListener(listener));
    protected virtual void StartInputListener(int p_id) => StartInputListener(InputManager.GetInputListener(p_id));
    protected virtual void StopInputListener(int p_id) => StopInputListener(InputManager.GetInputListener(p_id));

    protected virtual void StartInputListener(InputListener p_listener)
    {
        InputListener _inputListner = p_listener;
        
        _inputListner.UI.onWhileMovementActive += UI_onWhileMovementActive;
        _inputListner.UI.onWhileMovementActiveDelayed += UI_onWhileMovementActiveDelayed;
        _inputListner.UI.onConfirmRequested += UI_onConfirmRequested;
        _inputListner.UI.onCancelRequested += UI_InputHandler_onCancelRequested;
    }

    protected virtual void StopInputListener(InputListener p_listener)
    {
        InputListener _inputListner = p_listener;

        _inputListner.UI.onWhileMovementActive -= UI_onWhileMovementActive;
        _inputListner.UI.onWhileMovementActiveDelayed -= UI_onWhileMovementActiveDelayed;
        _inputListner.UI.onConfirmRequested -= UI_onConfirmRequested;
        _inputListner.UI.onCancelRequested -= UI_InputHandler_onCancelRequested;
    }

    protected virtual void UI_onWhileMovementActive(int p_id, Vector2 p_dir) => HandleMovement(false, p_id, p_dir);
    protected virtual void UI_onWhileMovementActiveDelayed(int p_id, Vector2 p_dir) => HandleMovement(true, p_id, p_dir);
    protected virtual void UI_onConfirmRequested(int p_id) { }
    protected virtual void UI_InputHandler_onCancelRequested(int p_id) { }
    protected virtual void HandleMovement(bool p_delayed, int p_id, Vector2 p_dir)
    {
        if (Mathf.Abs(p_dir.x) > 0.1f)
        {
            int __direction = p_dir.x > 0 ? 1 : -1;
            if (p_delayed) HandleHorizontalMovementDelayed(__direction); else HandleHorizontalMovementActive(__direction);

        }
        else if (Mathf.Abs(p_dir.y) > 0.1f)
        {
            int __direction = p_dir.y > 0 ? 1 : -1;

            if (p_delayed) HandleVerticalMovementDelayed(__direction); else HandleVerticalMovementActive(__direction);
        }
    }
    protected virtual void HandleHorizontalMovementActive(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnRight as UIOption : _curSelected.navigation.selectOnLeft as UIOption);
    protected virtual void HandleHorizontalMovementDelayed(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnRight as UIOption : _curSelected.navigation.selectOnLeft as UIOption);
    protected virtual void HandleVerticalMovementActive(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnUp as UIOption : _curSelected.navigation.selectOnDown as UIOption);
    protected virtual void HandleVerticalMovementDelayed(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnUp as UIOption : _curSelected.navigation.selectOnDown as UIOption);
    protected virtual void SelectOption(UIOption p_option)
    {
        if (p_option == null)
            return;

        SetCurSelected(p_option);
        _curSelected.Select();
        
    }
    protected virtual void SetCurSelected(UIOption p_option)
    {
        _curSelected = p_option;
    }

    #endregion

    public virtual void UpdateDisplay(int p_operation, bool p_value) { }
    public virtual void UpdateDisplay(int p_operation, float p_value, float p_data) { }
    public virtual void UpdateDisplay(int p_operation, float[] p_data){ }
    public virtual void UpdateDisplay(int p_operation, object p_data) { }

    public virtual object GetData(int p_data) { return null; }

    public virtual void RequestAction(int p_action) => onActionRequested?.Invoke(ID, p_action);
    public virtual void RequestAction(int p_action, object p_data) => onDataActionRequested?.Invoke(ID, p_action, p_data);
}
