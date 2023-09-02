using UnityEngine;

namespace ETemplate.UI
{
    public class WindowOption : SettingOption
    {
        public static FullScreenMode FullScreenMode;

        [SerializeField] private UIButton _leftButton, _rightButton;

        private int _totalModes = 0;

        public override void Initiate()
        {
            base.Initiate();
            _totalModes = System.Enum.GetValues(typeof(FullScreenMode)).Length;
        }

        public override void UpdateOptionActiveDelayed(int p_direction)
        {
            _index = HelpExtensions.ClampCircle(_index + p_direction, 0, _totalModes - 1);

            _valueText.text = ((FullScreenMode)_index).ToString();
        }

        public override void Apply()
        {
            base.Apply();
            FullScreenMode = (FullScreenMode)_index;
            Screen.SetResolution(Screen.width, Screen.height, (FullScreenMode)_index);
        }

        protected override void HandleSubscribeToEvents(bool p_subscribe)
        {
            _leftButton.onPointerClick.HandleSubscribe(() => UpdateOptionActiveDelayed(-1), p_subscribe);
            _rightButton.onPointerClick.HandleSubscribe(() => UpdateOptionActiveDelayed(1), p_subscribe);
            base.HandleSubscribeToEvents(p_subscribe);
        }
    }
}