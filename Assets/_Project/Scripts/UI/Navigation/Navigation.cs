using ETemplate.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETemplate.UI
{
    [RequireComponent(typeof(Display))]
    public class Navigation : MonoBehaviour
    {
        public const int BACK = 0;

        #region Events
        public UnityEvent<Displays> OnDisplayRequested { get; private set; } = new UnityEvent<Displays>();
        public UnityEvent<Displays> OnBackRequested { get; private set; } = new UnityEvent<Displays>();
        /// <summary>
        /// Called when the current input controller is moved on the horizontal axis (x).
        /// </summary>
        public UnityEvent<int> OnHorizontalNavigation { get; private set; } = new UnityEvent<int>();
        public UnityEvent<Selectable> OnSelected { get; private set; } = new UnityEvent<Selectable>();
        #endregion

        #region Fields
        [Tooltip("Use unitys default selectable navigation system")]
        [SerializeField] protected bool _useDefaultNavigation = true;
        [SerializeField] protected Selectable _startSelected;
        [Tooltip("Display it will return to when back button pressed")]
        [SerializeField] protected Displays _backTo;
        [SerializeField] protected UIButton _backButton;
        #endregion

        #region Getters
        public virtual Displays BackTo => _backTo;

        public Selectable CurSelected
        {
            get
            {
                GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject != null && selectedObject.TryGetComponent(out Selectable selectable))
                {
                    return selectable;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Vars, Properties(get&set)
        protected Display m_display;
        #endregion

        #region Utils
        public void Setup(Display display)
        {
            m_display = display;
            HandleSubscribeToEvents(true);
        }

        public void Dismantle() => HandleSubscribeToEvents(false);
        #endregion

        #region Events Listners
        protected void HandleOnShowStart()
        {
            if (_startSelected != null)
            {
                _startSelected.Select();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            if (_useDefaultNavigation)
            {
                InputManager.UIMainListener.onHoldingMovementDelayed += UIMainListener_OnHoldingMovementDelayed;
            }
        }

        protected void HandleOnHideStart()
        {
            if (_useDefaultNavigation)
            {
                InputManager.UIMainListener.onHoldingMovementDelayed -= UIMainListener_OnHoldingMovementDelayed;
            }
        }

        protected void UIMainListener_OnHoldingMovementDelayed(int p_id, Vector2 p_input) => DefaultNavigation(p_input);
        #endregion

        #region Internal
        protected void HandleSubscribeToEvents(bool subscribe)
        {
            foreach (var selectable in GetComponentsInChildren<UISelectable>())
            {
                selectable.onPointerEnter.HandleSubscribe(Select, subscribe);
            }

            m_display.OnShowStarted.HandleSubscribe(HandleOnShowStart, subscribe);
            m_display.OnHideStarted.HandleSubscribe(HandleOnHideStart, subscribe);

            _backButton?.onPointerClick.HandleSubscribe(HandleOnBackRequested, subscribe);
        }
        protected virtual void HandleOnBackRequested() => OnBackRequested?.Invoke(BackTo);
        protected void DefaultNavigation(Vector2 p_input)
        {
            if (CurSelected == null)
            {
                Debug.LogError("_curSelected is null");
                return;
            }

            if (Mathf.Abs(p_input.x) > 0.1f)
            {
                HorizontalNavigation(p_input.x);
            }
            else if (Mathf.Abs(p_input.y) > 0.1f)
            {
                VerticalNavigation(p_input.y);
            }
        }
        protected void HorizontalNavigation(float p_axis)
        {
            int __dir = p_axis > 0 ? 1 : -1;
            if (__dir > 0)
                Select(CurSelected.navigation.selectOnRight);
            else
                Select(CurSelected.navigation.selectOnLeft);

            OnHorizontalNavigation?.Invoke(__dir);
        }
        protected void VerticalNavigation(float p_axis)
        {
            int __dir = p_axis > 0 ? 1 : -1;
            if (__dir > 0)
                Select(CurSelected.navigation.selectOnUp);
            else
                Select(CurSelected.navigation.selectOnDown);
        }
        protected void Select(Selectable p_selectable)
        {
            if (p_selectable == null)
                return;

            p_selectable.Select();
            OnSelected?.Invoke(p_selectable);
        }
        #endregion
    }
}