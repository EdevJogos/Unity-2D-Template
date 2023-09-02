using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputHandler
{
    protected InputListener _listener;

    public virtual void Initiate(InputListener p_listener) => _listener = p_listener;
    public abstract void Initialize();
    public abstract void Tick();
    public abstract void Enable();
    public abstract void Disable();
}