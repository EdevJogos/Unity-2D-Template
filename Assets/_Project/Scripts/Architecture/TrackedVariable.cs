using System.Collections.Generic;
using UnityEngine;

public class TrackedVariable<T>
{
    private T _value;
    private List<System.Action<T>> _registeredActions = new List<System.Action<T>>();
    
    public T Value
    {
        get { return _value; }
        set
        {
            _value = value;
            
            InvokeActions(_value);
        }
    }

    public TrackedVariable(T p_value)
    {
        _value = p_value;
    }
    
    /// <summary>
    /// Set the value without a callback.
    /// </summary>
    /// <param name="p_value"></param>
    public void SetValue(T p_value)
    {
        _value = p_value;
    }

    public void Register(System.Action<T> p_action, int p_order)
    {
        _registeredActions.Add(p_action);
    }

    public void UnRegister(System.Action<T> p_action)
    {
        _registeredActions.Remove(p_action);
    }

    private void InvokeActions(T p_value)
    {
        for (int __i = 0; __i < _registeredActions.Count; __i++)
        {
            _registeredActions[__i].Invoke(p_value);
        }
    }
}
