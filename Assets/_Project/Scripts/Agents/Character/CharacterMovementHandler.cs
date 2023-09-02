using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementHandler : MonoBehaviour
{
    [SerializeField] private float _speed = 2;

    public Vector2 Direction { get; protected set; }
    public Rigidbody2D Body { get; protected set; }

    public void Setup(Rigidbody2D p_body)
    {
        Body = p_body;
    }

    public void Tick()
    {

    }

    public void FixedTick()
    {
        Body.velocity = Direction * _speed;
    }

    public void SetDirection(Vector2 p_direction)
    {
        Direction = p_direction;
    }
}
