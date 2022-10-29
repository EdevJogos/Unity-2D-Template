using UnityEngine;

public class InputManager : Manager
{
    public event System.Action onPauseRequested;

    public static Vector2 MouseWorld { get { return CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition); } }

    public override void Initiate()
    {
        
    }

    public override void Initialize()
    {
        
    }

    public override void Renew()
    {
        
    }
}