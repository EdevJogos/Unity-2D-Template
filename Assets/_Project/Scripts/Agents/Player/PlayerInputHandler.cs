using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public event System.Action<Vector2> onMovementPerformed;

    private InputListener _inputListener;

    public void Setup(InputListener p_listener)
    {
        _inputListener = p_listener;
    }

    public void Intiate()
    {
        _inputListener.Player.onMovementPerformed += Player_onMovementPerformed;
    }

    private void Player_onMovementPerformed(Vector2 p_input)
    {
        onMovementPerformed?.Invoke(p_input);
    }
}
