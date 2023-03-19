using UnityEngine;
using UnityEngine.UI;

public class SettingOption : UIOption
{
    public Vector2 Position => transform.position;

    [SerializeField] protected TMPro.TextMeshProUGUI _valueText;

    protected int _index = 0;
    public virtual void Initiate() { }
    public virtual void UpdateCurrentValues() { }
    public virtual void UpdateOptionActive(int p_direction) { }
    public virtual void UpdateOptionActiveDelayed(int p_direction) { }

    public virtual void Apply() { }
    public virtual void Cancel() { }
}
