using UnityEngine;
using System;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{
    public Transform displaysHolder;

    private Display _activeDisplay;
    private Dictionary<Displays, Display> _displays = new Dictionary<Displays, Display>();

    public void Initiate()
    {
        Display.onActionRequested += OnActionRequested;

        foreach (Transform __transform in displaysHolder)
        {
            Display __display = __transform.GetComponent<Display>();

            if (__display == null)
                return;

            __display.Initiate();
            _displays.Add(__display.ID, __display);
        }
    }

    private void OnDestroy()
    {
        Display.onActionRequested -= OnActionRequested;
    }

    public void Initialize()
    {
        foreach (Display __display in _displays.Values)
        {
            __display.Initialize();
        }
    }

    public void ShowDisplay(Displays p_display, Action p_onShowCompleted = null, float p_hideRatio = 1f, float p_showRatio = 1f)
    {
        if (_activeDisplay == null || (_activeDisplay != null && _activeDisplay.ID != p_display))
        {
            if (_activeDisplay != null)
            {
                _activeDisplay.Show(false, () => { ActiveDisplay(p_display, p_onShowCompleted, p_showRatio); }, p_hideRatio);
            }
            else
            {
                ActiveDisplay(p_display, p_onShowCompleted, p_showRatio);
            }
        }
    }

    private void ActiveDisplay(Displays p_display, Action p_onShowCompleted, float p_showRatio)
    {
        _activeDisplay = _displays[p_display];
        _activeDisplay.Show(true, p_onShowCompleted, p_showRatio);
    }

    #region Update Display Calls

    public void UpdateDisplay(Displays p_id, int p_operation, bool p_value)
    {
        _displays[p_id].UpdateDisplay(p_operation, p_value);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, float p_value = -99999, float p_data = -99999)
    {
        _displays[p_id].UpdateDisplay(p_operation, p_value, p_data);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, int[] p_data)
    {
        _displays[p_id].UpdateDisplay(p_operation, p_data);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, object p_data)
    {
        _displays[p_id].UpdateDisplay(p_operation, p_data);
    }

    #endregion

    public object GetData(Displays p_id, int p_data)
    {
        return _displays[p_id].GetData(p_data);
    }

    private void OnActionRequested(Displays p_id, int p_action)
    {
        switch (p_id)
        {
            case Displays.INTRO:
                switch (p_action)
                {
                    case 0:
                        break;
                }
                break;
        }
    }
}