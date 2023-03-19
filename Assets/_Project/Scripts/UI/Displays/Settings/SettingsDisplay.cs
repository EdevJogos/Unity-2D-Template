using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDisplay : Display
{
    public const int BACK = 0;

    public Transform selection;
    public GridLayoutGroup optionsGrid;

    private List<SettingOption> _options = new List<SettingOption>();

    private SettingOption SelectedOption => _curSelected as SettingOption;

    public override void Initialize()
    {
        foreach (Transform __transform in optionsGrid.transform)
        {
            if (__transform.TryGetComponent(out SettingOption __settingOption))
            {
                __settingOption.onSelected += () => { selection.position = __settingOption.Position; SetCurSelected(__settingOption); };
                __settingOption.Initiate();

                _options.Add(__settingOption);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsGrid.GetComponent<RectTransform>());

        base.Initialize();
    }

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        if (p_show)
        {
            for (int __i = 0; __i < _options.Count; __i++)
            {
                _options[__i].UpdateCurrentValues();
            }
        }
        else
        {
            PlayerPrefs.Save();
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    protected override void HandleHorizontalMovementActive(int p_dir) => SelectedOption.UpdateOptionActive(p_dir);
    protected override void HandleHorizontalMovementDelayed(int p_dir) => SelectedOption.UpdateOptionActiveDelayed(p_dir);
    protected override void HandleVerticalMovementActive(int p_dir) { }

    protected override void UI_onConfirmRequested(int p_id)
    {
        Debug.Log("Confirm");
        for (int __i = 0; __i < _options.Count; __i++)
        {
            _options[__i].Apply();
        }
    }

    protected override void UI_InputHandler_onCancelRequested(int p_id)
    {
        RequestAction(BACK);
    }
}