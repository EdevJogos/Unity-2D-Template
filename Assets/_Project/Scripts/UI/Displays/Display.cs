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

    public Displays ID => id;

    [SerializeField] protected DisplayTypes type;
    [SerializeField] protected Displays id;
    [SerializeField] protected AnimationStyles animationStyle;
    [Tooltip("When true it will listen to all players inputs once the display is active")]
    [SerializeField] protected bool _receiveInputWhenActive = true;
    [Tooltip("When true it will listen to the whileActive callback, which is called every frame while move input != 0, override it on the display to work only certain cases if needed")]
    [SerializeField] protected bool _useWhileActiveInput = false;
    [SerializeField] protected UIOption _startSelected;

    protected UIOption _curSelected;

    public virtual void Initiate() { }
    public virtual void Initialize() { }

    public virtual void Show(bool p_show, System.Action p_onCompleted, float p_ratio)
    {
        if (p_show)
        {
            if(_receiveInputWhenActive) StartInputListeners();

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
                GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = p_show;
                p_onCompleted?.Invoke();
                break;
            case AnimationStyles.ANIMATOR:
                HandleAnimator(p_show, p_onCompleted, p_ratio);
                break;
            case AnimationStyles.TWEEN:
                HandleTween(p_show, p_onCompleted);
                break;
        }
    }

    #region Animator
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
    #endregion

    #region Tween
    protected virtual void HandleTween(bool p_show, System.Action p_onCompleted)
    {
        if (p_show) TweenInAnimation(p_onCompleted); else TweenOutAnimation(p_onCompleted);
    }
    /// <summary>
    /// Override this to write the animation that will be executed when ShowDisplay is called.
    /// base will call the onCompleted callback, be careful to not end up calling it twiece.
    /// </summary>
    protected virtual void TweenInAnimation(System.Action p_onCompleted) { p_onCompleted?.Invoke(); }
    /// <summary>
    /// Override this to write the animation that will be executed when HideDisplay is called.
    /// base will call the onCompleted callback, be careful to not end up calling it twiece.
    /// </summary>
    protected virtual void TweenOutAnimation(System.Action p_onCompleted) { p_onCompleted?.Invoke(); }
    #endregion

    #region Inputs

    #region Start InputListeners / Assing Input Callbacks
    /// <summary>
    /// The current Display wwill start receiving input from all players.
    /// </summary>
    protected virtual void StartInputListeners() => InputManager.InputListeners.ForEach(listener => StartInputListener(listener));
    protected virtual void StopInputListeners() => InputManager.InputListeners.ForEach(listener => StopInputListener(listener));
    /// <summary>
    /// The current Display wwill start receiving input from the players with the p_id (the player listner id is it's position on the _inputListeners List at InputManager).
    /// </summary>
    protected virtual void StartInputListener(int p_id) => StartInputListener(InputManager.GetInputListener(p_id));
    protected virtual void StopInputListener(int p_id) => StopInputListener(InputManager.GetInputListener(p_id));

    /// <summary>
    /// Subscribes to all UI input events from p_listener
    /// </summary>
    protected virtual void StartInputListener(InputListener p_listener)
    {
        InputListener _inputListner = p_listener;
        
        if(_useWhileActiveInput) _inputListner.UI.onWhileMovementActive += UI_onWhileMovementActive;
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
    #endregion

    #region Input Callbacks
    /// <summary>
    /// It runs on a update so while the input is != then 0 this will be running. (use when constant update is required)
    /// </summary>
    protected virtual void UI_onWhileMovementActive(int p_id, Vector2 p_dir) => HandleMovement(false, p_id, p_dir);
    /// <summary>
    ///  It runs on a update while the input is != then 0, but has a delay in between calls default delay is 0.25s.
    /// </summary>
    protected virtual void UI_onWhileMovementActiveDelayed(int p_id, Vector2 p_dir) => HandleMovement(true, p_id, p_dir);
    protected virtual void UI_onConfirmRequested(int p_id) { }
    protected virtual void UI_InputHandler_onCancelRequested(int p_id) { }
    #endregion

    #region Callback Handlers
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
    //It uses the Editor Navigation to define the order in which the elemets will be selected when an input is made, assing it on the editor for each UIOption.
    //All UI Selectables must be an UIOption or derive from it.
    protected virtual void HandleHorizontalMovementActive(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnRight as UIOption : _curSelected.navigation.selectOnLeft as UIOption);
    protected virtual void HandleHorizontalMovementDelayed(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnRight as UIOption : _curSelected.navigation.selectOnLeft as UIOption);
    protected virtual void HandleVerticalMovementActive(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnUp as UIOption : _curSelected.navigation.selectOnDown as UIOption);
    protected virtual void HandleVerticalMovementDelayed(int p_direction) => SelectOption(p_direction > 0 ? _curSelected.navigation.selectOnUp as UIOption : _curSelected.navigation.selectOnDown as UIOption);
    public virtual void SelectOption(UIOption p_option)
    {
        if (p_option == null)
            return;

        _curSelected = p_option;
        _curSelected.Select();
    }
    #endregion

    #endregion

    public virtual void UpdateDisplay(int p_operation, bool p_value) { }
    public virtual void UpdateDisplay(int p_operation, float p_value, float p_data) { }
    public virtual void UpdateDisplay(int p_operation, float[] p_data){ }
    public virtual void UpdateDisplay(int p_operation, object p_data) { }

    public virtual object GetData(int p_data) { return null; }

    public virtual void RequestAction(int p_action) => onActionRequested?.Invoke(ID, p_action);
    public virtual void RequestAction(int p_action, object p_data) => onDataActionRequested?.Invoke(ID, p_action, p_data);
}
