using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class ResolutionOption : SettingOption
{
    private Resolution[] _availableResolutions;
    private Resolution _choosenResolution, _backToResolution;

    protected override void Awake()
    {
        base.Awake();

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
}
