using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event System.Action onPauseRequested;

    public static Vector2 MouseWorld { get { return CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition); } }
}