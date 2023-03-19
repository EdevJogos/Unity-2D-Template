using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputManager : Manager
{
    public static InputManager Instance;
    public static Vector2 MouseWorld { get { return CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition); } }
    public static List<InputListener> InputListeners => Instance._inputListeners;

    public event Action onPlayerJoined;
    public event Action onPlayerLeft;

    [SerializeField] private InputMaps _startInputMap;
    [SerializeField, ReadyOnly] private InputMaps _curInputMap;

    private List<InputListener> _inputListeners = new List<InputListener>(4);

    public override void Initiate()
    {
        Instance = this;

        CreateInputListener();
    }

    public override void Initialize()
    {
        SwitchInputHandler(InputMaps.UI);
    }

    private void Update()
    {
        for (int __i = 0; __i < _inputListeners.Count; __i++)
        {
            _inputListeners[__i].Tick();
        }
    }

    public override void Renew()
    {
        
    }

    public void Join(InputDevice p_device)
    {

    }

    public void Leave(InputDevice p_device)
    {

    }

    public void SwitchInputHandler(InputMaps p_handler)
    {

    }

    private InputListener CreateInputListener()
    {
        InputListener __inputListener = new InputListener();

        _inputListeners.Add(__inputListener);

        __inputListener.Initiate();
        __inputListener.Initialize();

        return __inputListener;
    }

    public static InputListener GetInputListener(int p_id)
    {
        return Instance._inputListeners[p_id];
    }

    public static int GetInputID(InputListener p_listner)
    {
        return Instance._inputListeners.IndexOf(p_listner);
    }
}