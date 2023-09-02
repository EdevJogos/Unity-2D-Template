using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace ETemplate.Manager
{
    public class InputManager : Manager
    {
        public static InputManager Instance;
        public static Vector2 MouseWorld { get { return CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition); } }
        public static UIInputs UIMainListener => Instance._inputListeners[0].UI;
        public static List<InputListener> InputListeners => Instance._inputListeners;

        public event Action<int> onPlayerJoined;
        public event Action onPlayerLeft;

        public GeneralIO GeneralIO { get; private set; }

        [SerializeField] private InputMaps _startInputMap;
        [SerializeField, ReadyOnly] private InputMaps _curInputMap;

        private List<InputListener> _inputListeners = new List<InputListener>(4);

        public override void Initiate()
        {
            Instance = this;

            GeneralIO = new GeneralIO();
            GeneralIO.Enable();

            CreateInputListener();
        }

        public override void Initialize()
        {
            SwitchInputMap(InputMaps.UI);
        }

        private void Update()
        {
            for (int __i = 0; __i < _inputListeners.Count; __i++)
            {
                _inputListeners[__i].Tick();
            }
        }

        public void Join(InputDevice p_device)
        {
            _inputListeners[0].SetKeyboardAndMouse();
            CreateInputListener().SetDevice(p_device);

            onPlayerJoined?.Invoke(_inputListeners.Count - 1);
        }

        public void Leave(InputDevice p_device)
        {

        }

        public void SwitchInputMap(InputMaps p_map)
        {
            foreach (InputListener __inputListener in _inputListeners)
            {
                __inputListener.SwitchInputMap(p_map);
            }
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
}