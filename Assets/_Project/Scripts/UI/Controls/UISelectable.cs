using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace ETemplate.UI
{
    public class UISelectable : Selectable, ISubmitHandler, IPointerClickHandler
    {
        public UnityEvent onPointerClick;
        public UnityEvent<UISelectable> onPointerEnter;
        public UnityEvent onSelect;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onPointerClick.RemoveAllListeners();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            onSelect?.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Select();
            onPointerEnter?.Invoke(this);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            onPointerClick?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke();
        }
    }
}