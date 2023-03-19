using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputListener
{
    public InputsIO InputsIO { protected set; get; }
    public UIInputs UI { get; private set; }

    private InputHandler _curInput;

    public void Initiate()
    {
        InputsIO = new InputsIO();
        UI = new UIInputs();

        InputsIO.UI.Enable();
        InputsIO.UI.Movement.performed += UI.MovementPerformed;
        InputsIO.UI.Confirm.performed += UI.ConfirmPerformed;
        InputsIO.UI.Cancel.performed += UI.CancelPerformed;

        _curInput = UI;
        _curInput.Initiate(this);
    }
    public void Initialize() => _curInput.Initialize();
    public void Tick() => _curInput.Tick();
    public void Enable() => _curInput.Enable();
    public void Disable() => _curInput.Disable();
    public void SetDevice(InputDevice p_device)
    {
        
    }
}



public class UIInputs : InputHandler
{
    public event Action<int, Vector2> onWhileMovementActive;
    public event Action<int, Vector2> onWhileMovementActiveDelayed;
    public event Action<int> onConfirmRequested;
    public event Action<int> onCancelRequested;

    private float _moveDelay = 0.0f;
    private Vector2 _moveInput;
    private InputListener _listener;

    public override void Initiate(InputListener p_listener)
    {
        _listener = p_listener;
    }

    public override void Initialize()
    {
        
    }
    public override void Tick()
    {
        if(_moveInput.magnitude > 0)
        {
            _moveDelay -= Time.deltaTime;

            if (_moveDelay <= 0f)
            {
                onWhileMovementActiveDelayed?.Invoke(InputManager.GetInputID(_listener), _moveInput);
                _moveDelay = 0.25f;
            }

            onWhileMovementActive?.Invoke(InputManager.GetInputID(_listener), _moveInput);
        }
    }
    public override void Enable()
    {
        
    }
    public override void Disable()
    {
        
    }

    public void MovementPerformed(InputAction.CallbackContext p_context)
    {
        _moveDelay = 0.0f;
        _moveInput = p_context.ReadValue<Vector2>();
    }

    public void ConfirmPerformed(InputAction.CallbackContext p_context) => onConfirmRequested?.Invoke(InputManager.GetInputID(_listener));
    public void CancelPerformed(InputAction.CallbackContext p_context) => onCancelRequested?.Invoke(InputManager.GetInputID(_listener));
}