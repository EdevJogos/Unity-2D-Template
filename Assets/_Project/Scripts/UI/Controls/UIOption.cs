using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIOption : Selectable, ISubmitHandler
{
    [SerializeField] private UnityEvent _onPointerUp;

    public event System.Action onSelected;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelected?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Select();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _onPointerUp?.Invoke();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _onPointerUp?.Invoke();
    }
}
