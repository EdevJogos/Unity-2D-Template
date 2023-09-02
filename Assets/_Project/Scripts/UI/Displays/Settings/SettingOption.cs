using UnityEngine;
using UnityEngine.UI;
using ETemplate.UI;

namespace ETemplate.UI
{
    public class SettingOption : UISelectable
    {
        public Vector2 Position => transform.position;

        [SerializeField] protected TMPro.TextMeshProUGUI _valueText;

        protected int _index = 0;
        public virtual void Initiate() => HandleSubscribeToEvents(true);
        public virtual void UpdateCurrentValues() { }
        public virtual void UpdateOptionActive(int p_direction) { }
        public virtual void UpdateOptionActiveDelayed(int p_direction) { }
        public virtual void Apply() { }
        public virtual void Cancel() { }

        protected override void OnDestroy()
        {
            HandleSubscribeToEvents(false);
            base.OnDestroy();
        }
        protected virtual void HandleSubscribeToEvents(bool p_subscribe) { }
    }
}