using System;
using UnityEngine;

public class TrackedAction
{
    public string label;
    public Action action;

    public TrackedAction(string p_label, Action p_action)
    {
        label = p_label;
        action = p_action;
    }

    public void Invoke()
    {
        action.Invoke();
    }
}