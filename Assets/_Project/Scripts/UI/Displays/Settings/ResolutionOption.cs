using UnityEngine;

namespace ETemplate.UI
{
    public class ResolutionOption : SettingOption
    {
        [SerializeField] private UIButton _leftButton, _rightButton;

        private Resolution[] _availableResolutions;
        private Resolution _choosenResolution, _backToResolution;

        public override void Initiate()
        {
            base.Initiate();
            _availableResolutions = Screen.resolutions;
        }

        public override void UpdateCurrentValues()
        {
            base.UpdateCurrentValues();

            _index = System.Array.IndexOf(_availableResolutions, Screen.currentResolution);
            _backToResolution = Screen.currentResolution;
            UpdateChoosenResolution(Screen.currentResolution);
        }

        public override void UpdateOptionActiveDelayed(int p_direction)
        {
            _index = HelpExtensions.ClampCircle(_index + p_direction, 0, _availableResolutions.Length - 1);

            UpdateChoosenResolution(_availableResolutions[_index]);
        }

        public override void Apply()
        {
            base.Apply();
            Screen.SetResolution(_choosenResolution.width, _choosenResolution.height, WindowOption.FullScreenMode);
        }

        public override void Cancel()
        {
            base.Cancel();
            UpdateChoosenResolution(_backToResolution);
        }

        private void UpdateChoosenResolution(Resolution p_choosen)
        {
            _choosenResolution = p_choosen;
            _valueText.text = p_choosen.width + "x" + p_choosen.height;
        }

        protected override void HandleSubscribeToEvents(bool p_subscribe)
        {
            _leftButton.onPointerClick.HandleSubscribe(() => UpdateOptionActiveDelayed(-1), p_subscribe);
            _rightButton.onPointerClick.HandleSubscribe(() => UpdateOptionActiveDelayed(1), p_subscribe);
            base.HandleSubscribeToEvents(p_subscribe);
        }
    }
}