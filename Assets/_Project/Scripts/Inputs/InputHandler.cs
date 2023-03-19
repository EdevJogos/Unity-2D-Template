using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputHandler
{
    public abstract void Initiate(InputListener p_listner);
    public abstract void Initialize();
    public abstract void Tick();
    public abstract void Enable();
    public abstract void Disable();
}