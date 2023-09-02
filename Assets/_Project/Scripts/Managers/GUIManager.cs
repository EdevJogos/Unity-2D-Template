using UnityEngine;
using System;
using System.Collections.Generic;

namespace ETemplate.Manager
{
    [Serializable]
    public class GUIManager : Manager
    {
        public event Action onIntroDisplayRequested;
        public event Action onCreditsDisplayRequested;
        public event Action onSettingsDisplayRequested;

        [SerializeField] private Transform _displaysHolder;

        private UI.Display _activeDisplay;
        private Dictionary<Displays, UI.Display> _displays = new Dictionary<Displays, UI.Display>();

        public override void Initiate()
        {
            foreach (Transform __transform in _displaysHolder)
            {
                if(__transform.TryGetComponent(out UI.Display __display))
                {
                    __display.Initiate();
                    _displays.Add(__display.ID, __display);
                }
            }
        }

        public override void Initialize()
        {
            HandleDisplayEvents(true);

            foreach (UI.Display __display in _displays.Values)
            {
                __display.Initialize();
            }
        }

        private void OnDestroy()
        {
            HandleDisplayEvents(false);
        }

        public void ShowDisplay(Displays p_id, Action p_onShowCompleted = null, float p_hideRatio = 1f, float p_showRatio = 1f)
        {
            if (_activeDisplay == null || (_activeDisplay != null && _activeDisplay.ID != p_id))
            {
                if (_activeDisplay != null)
                {
                    _activeDisplay.Show(false, () => { ActiveDisplay(p_id, p_onShowCompleted, p_showRatio); }, p_hideRatio);
                }
                else
                {
                    ActiveDisplay(p_id, p_onShowCompleted, p_showRatio);
                }
            }
        }

        public UI.Display GetDisplay(Displays p_id)
        {
            return _displays[p_id];
        }

        public T GetDisplay<T>(Displays p_id) where T : class
        {
            return GetDisplay(p_id).GetComponent<T>();
        }

        private void RequestDisplay(Displays p_id)
        {
            switch (p_id)
            {
                case Displays.INTRO: onIntroDisplayRequested?.Invoke(); break;
                case Displays.CREDITS: onCreditsDisplayRequested?.Invoke(); break;
                case Displays.SETTINGS: onSettingsDisplayRequested?.Invoke(); break;
            }
        }

        private void HandleDisplayEvents(bool p_subscribe)
        {
            foreach (UI.Display __display in _displays.Values)
            {
                __display.Navigation?.OnDisplayRequested.HandleSubscribe(RequestDisplay, p_subscribe);
                __display.Navigation?.OnBackRequested.HandleSubscribe(RequestDisplay, p_subscribe);
            }
        }

        private void ActiveDisplay(Displays p_display, Action p_onShowCompleted, float p_showRatio)
        {
            _activeDisplay = _displays[p_display];
            _activeDisplay.Show(true, p_onShowCompleted, p_showRatio);
        }
    } 
}