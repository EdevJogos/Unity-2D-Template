using System.Collections.Generic;
using UnityEngine;

public class TrackedEvent
{
#if UNITY_EDITOR
    public List<TrackedAction> trackedActions = new List<TrackedAction>();
#endif

    private List<System.Action> _registeredActions = new List<System.Action>();

    public GameEvents ID;

    public TrackedEvent(GameEvents p_id)
    {
        ID = p_id;
    }

    public void Register(System.Action p_action, int p_order)
    {
#if UNITY_EDITOR
        string __caller = new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType.ToString() + " Method: " + p_action.Method.Name;
        trackedActions.Add(new TrackedAction(__caller, p_action));
#endif

        _registeredActions.Add(p_action);
    }

    public void UnRegister(System.Action p_action)
    {
        _registeredActions.Remove(p_action);
    }

    public void Invoke()
    {
        for (int __i = 0; __i < _registeredActions.Count; __i++)
        {
            _registeredActions[__i]?.Invoke();
        }
    }
}