using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIOption : Selectable, ISubmitHandler
{
    public UnityEvent onPointerUp;

    public event System.Action onSelected;

    private Display m_display;

    private Display Display
    {
        get
        {
            if(m_display == null)
            {
                m_display = GetComponentInParent<Display>();
            }
            return m_display;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        onPointerUp.RemoveAllListeners();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelected?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Display.SelectOption(this);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointerUp?.Invoke();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onPointerUp?.Invoke();
    }
}
