﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GUIManager : Manager
{
    public struct DisplayAction
    {
        public int actionID;
        public Action<object> action;

        public DisplayAction(int p_id, Action<object> p_action)
        {
            actionID = p_id;
            action = p_action;
        }
    }

    public event Action<object> onJoinRequested;

    [SerializeField] private Transform _displaysHolder;

    private Display _activeDisplay;
    private Dictionary<Displays, Display> _displays = new Dictionary<Displays, Display>();
    private Dictionary<Displays, List<DisplayAction>> _displayActions = new Dictionary<Displays, List<DisplayAction>>();

    public override void Initiate()
    {
        Display.onActionRequested += OnActionRequested;

        foreach (Transform __transform in _displaysHolder)
        {
            Display __display = __transform.GetComponent<Display>();

            if (__display == null)
                return;

            __display.Initiate();
            _displays.Add(__display.ID, __display);
            _displayActions.Add(__display.ID, new List<DisplayAction>());
        }

        _displayActions[Displays.INTRO].Add(new DisplayAction(1, (p_data) => { ShowDisplay(Displays.LOBBY); }));
        _displayActions[Displays.INTRO].Add(new DisplayAction(2, (p_data) => { ShowDisplay(Displays.SETTINGS); }));

        _displayActions[Displays.SETTINGS].Add(new DisplayAction(0, (p_data) => { ShowDisplay(Displays.INTRO); }));

        _displayActions[Displays.LOBBY].Add(new DisplayAction(0, (p_data) => { ShowDisplay(Displays.INTRO); }));
        _displayActions[Displays.LOBBY].Add(new DisplayAction(LobbyDisplay.JOIN, onJoinRequested));
    }

    private void OnDestroy()
    {
        Display.onActionRequested -= OnActionRequested;
    }

    public override void Initialize()
    {
        foreach (Display __display in _displays.Values)
        {
            __display.Initialize();
        }
    }

    public override void Renew()
    {
        
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

    private void OnActionRequested(Displays p_id, int p_actionID, object p_data)
    {
        List<DisplayAction> __actionsList = _displayActions[p_id];
        int __index = __actionsList.FindIndex(0, __actionsList.Count, (displayAction) => displayAction.actionID == p_actionID);

        if(__index >= 0)
        {
            __actionsList[__index].action?.Invoke(p_data);
        }
    }
}