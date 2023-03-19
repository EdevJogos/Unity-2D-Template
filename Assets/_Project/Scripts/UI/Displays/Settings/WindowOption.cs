using UnityEngine;

public class WindowOption : SettingOption
{
    public static FullScreenMode FullScreenMode;

    private int _totalModes = 0;

    protected override void Awake()
    {
        base.Awake();
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
}
