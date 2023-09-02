using ETemplate.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETemplate.UI
{
    public class SettingsDisplay : Display
    {
        public Image imageSelection;
        public GridLayoutGroup optionsGrid;

        private List<SettingOption> _options = new List<SettingOption>();

        private SettingOption SelectedOption => Navigation.CurSelected as SettingOption;

        public override void Initiate()
        {
            foreach (Transform __transform in optionsGrid.transform)
            {
                if (__transform.TryGetComponent(out SettingOption __settingOption))
                {
                    __settingOption.Initiate();
                    _options.Add(__settingOption);
                }
            }

            base.Initiate();
        }

        public override void Initialize()
        {
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

                imageSelection.enabled = true;

                InputManager.UIMainListener.onHoldingMovement += UIMainListener_onHoldingMovement;
            }
            else
            {
                InputManager.UIMainListener.onHoldingMovement -= UIMainListener_onHoldingMovement;
                PlayerPrefs.Save();
            }

            Navigation.OnSelected.HandleSubscribe(HandleOnSelected, p_show);
            Navigation.OnHorizontalNavigation.HandleSubscribe(HandleHorizontalNavigation, p_show);

            base.Show(p_show, p_callback, p_ratio);
        }

        public void ApplySettings()
        {
            for (int __i = 0; __i < _options.Count; __i++)
            {
                _options[__i].Apply();
            }
        }

        public void HandleOnSelected(Selectable p_selectable)
        {
            //Moves the selection bar to the current selected button.
            if (p_selectable is SettingOption)
            {
                imageSelection.enabled = true;
                imageSelection.transform.position = p_selectable.transform.position;
            }
            else imageSelection.enabled = false;
        }

        //Calls UpdateOptionActiveDelayed to update the selection options like (Resolution and Window mode).
        public void HandleHorizontalNavigation(int p_dir) => SelectedOption.UpdateOptionActiveDelayed(p_dir);
        //Uses onHoldingMovement to call UpdateOptionActive and update the volume sliders since they need to move x amount every frame.
        private void UIMainListener_onHoldingMovement(int p_id, Vector2 p_input)
        {
            if (Mathf.Abs(p_input.x) > 0.1f)
            {
                int __dir = p_input.x > 0 ? 1 : -1;
                SelectedOption.UpdateOptionActive(__dir);
            }
        }
    }
}