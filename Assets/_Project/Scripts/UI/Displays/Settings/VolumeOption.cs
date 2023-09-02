using ETemplate.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace ETemplate.UI
{
    public class VolumeOption : SettingOption
    {
        [SerializeField] private float _sliderSensibility;
        [SerializeField] private Slider _valueSlider;
        [Tooltip("Channels: 0 Master, 1 Music, 2 SFX")]
        [SerializeField] private int _channel;

        private float _volume = 100;

        public override void Initiate()
        {
            base.Initiate();
            _valueSlider.onValueChanged.AddListener(UpdateVolume);
        }

        public override void UpdateCurrentValues()
        {
            base.UpdateCurrentValues();
            UpdateUIValue();
        }

        public override void UpdateOptionActive(int p_direction)
        {
            UpdateVolume(_volume + (p_direction * _sliderSensibility * Time.deltaTime));
            AudioManager.SetVolume(_channel, _volume * 0.01f);
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        private void UpdateVolume(float p_volume)
        {
            _volume = Mathf.Clamp(p_volume, 0.1f, 100);
            UpdateUIValue();
        }

        private void UpdateUIValue()
        {
            _valueText.text = Mathf.FloorToInt(_volume) + "%";
            _valueSlider.SetValueWithoutNotify(_volume);
        }
    }
}