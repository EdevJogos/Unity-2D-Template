using UnityEngine;
using System;
using UnityEngine.InputSystem;
using ETemplate.Manager;

public class InputListener
{
    public InputsIO InputsIO { protected set; get; }
    public UIInputs UI { get; private set; }
    public PlayerInputs Player { get; private set; }

    private InputHandler _curInput;

    public void Initiate()
    {
        InputsIO = new InputsIO();
        UI = new UIInputs();
        Player = new PlayerInputs();

        InputsIO.UI.Enable();
        InputsIO.UI.Movement.performed += UI.MovementPerformed;
        InputsIO.UI.Movement.canceled += UI.MovementCanceled;
        InputsIO.UI.Confirm.performed += UI.ConfirmPerformed;
        InputsIO.UI.Cancel.performed += UI.CancelPerformed;

        InputsIO.Player.Movement.performed += Player.MovementPerformed;
        InputsIO.Player.Movement.canceled += Player.MovementCanceled;

        UI.Initiate(this);
        Player.Initiate(this);

        _curInput = UI;
    }

    public void Initialize()
    {
        UI.Initialize();
        Player.Initialize();
    }
    public void Tick() => _curInput.Tick();
    public void SetDevice(InputDevice p_device)
    {
        InputsIO.devices = new InputDevice[] { p_device };
    }

    public void SetKeyboardAndMouse()
    {
        InputsIO.devices = new InputDevice[] { Keyboard.current, Mouse.current };
    }

    public void SwitchInputMap(InputMaps p_map)
    {
        _curInput?.Disable();

        switch(p_map)
        {
            case InputMaps.UI: _curInput = UI; break;
            case InputMaps.PLAYER: _curInput = Player; break;
        }

        _curInput.Enable();
    }
}

public class UIInputs : InputHandler
{
    /// <summary>
    /// Called every frame while the movement is holded.
    /// </summary>
    public event Action<int, Vector2> onHoldingMovement;
    /// <summary>
    /// Call every x seconds while the movement is holded.
    /// </summary>
    public event Action<int, Vector2> onHoldingMovementDelayed;
    public event Action<int> onConfirmRequested;
    public event Action<int> onCancelRequested;

    private float _moveDelay = 0.0f;
    private Vector2 _moveInput;

    public override void Initialize()
    {
        
    }
    public override void Tick()
    {
        if(_moveInput.magnitude >= 0.1f)
        {
            _moveDelay -= Time.deltaTime;

            if (_moveDelay <= 0f)
            {
                onHoldingMovementDelayed?.Invoke(InputManager.GetInputID(_listener), _moveInput);
                _moveDelay = 0.25f;
            }

            onHoldingMovement?.Invoke(InputManager.GetInputID(_listener), _moveInput);
        }
    }

    public override void Enable() => _listener.InputsIO.UI.Enable();
    public override void Disable() => _listener.InputsIO.UI.Disable();

    public void MovementPerformed(InputAction.CallbackContext p_context)
    {
        if(_moveInput.magnitude < 0.1f) 
            _moveDelay = 0.0f;

        _moveInput = p_context.ReadValue<Vector2>();
    }

    public void MovementCanceled(InputAction.CallbackContext p_context)
    {
        _moveDelay = 0.0f;
        _moveInput = Vector2.zero;
    }

    public void ConfirmPerformed(InputAction.CallbackContext p_context) => onConfirmRequested?.Invoke(InputManager.GetInputID(_listener));
    public void CancelPerformed(InputAction.CallbackContext p_context) => onCancelRequested?.Invoke(InputManager.GetInputID(_listener));
}

public class PlayerInputs : InputHandler
{
    public event Action<Vector2> onMovementPerformed;

    public override void Initialize()
    {
        
    }
    public override void Tick()
    {
        
    }
    public override void Enable() => _listener.InputsIO.Player.Enable();
    public override void Disable() => _listener.InputsIO.Player.Disable();

    public void MovementPerformed(InputAction.CallbackContext p_context) => onMovementPerformed?.Invoke(p_context.ReadValue<Vector2>());
    public void MovementCanceled(InputAction.CallbackContext p_context) => onMovementPerformed?.Invoke(Vector2.zero);

}