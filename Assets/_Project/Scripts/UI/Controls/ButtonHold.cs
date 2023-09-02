using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETemplate.UI
{
    public class ButtonHold : UIButton
    {
        public bool Charged { get; protected set; } = false;
        public float CurCharge { get; protected set; } = 0f;

        [SerializeField] private Image _progressImage;
        [SerializeField] private bool _resetWhenReleased = true;
        [Tooltip("Time in seconds")]
        [SerializeField] private float _maxCharge;

        public UnityEvent<float> onCharging;
        public UnityEvent onCharged;

        private bool _charging = false;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onCharging.RemoveAllListeners();
            onCharged.RemoveAllListeners();
        }

        private void Update()
        {
            if (!_charging)
                return;

            CurCharge = HelpExtensions.ClampMax(CurCharge + Time.deltaTime, _maxCharge);
            _progressImage.fillAmount = HelpExtensions.ClampMax(CurCharge / _maxCharge, 1f);
            onCharging?.Invoke(CurCharge);

            if (CurCharge == _maxCharge)
            {
                Charged = true;
                StopCharging();
                Debug.Log("E: Charged");
                onCharged?.Invoke();
            }
        }

        public void Restart()
        {
            Charged = false;
            CurCharge = 0f;
            _charging = false;
            _progressImage.fillAmount = 0;
        }

        private void StartCharging()
        {
            if (Charged)
                return;

            _charging = true;
        }

        private void StopCharging()
        {
            _charging = false;

            if (_resetWhenReleased && !Charged)
            {
                Restart();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            StartCharging();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            StopCharging();
        }
    }
}
