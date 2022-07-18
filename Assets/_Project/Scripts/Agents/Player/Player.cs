using UnityEngine;

public class Player : Character
{
    public PlayerInputsHandler _playerInputHandler;

    protected override void Awake()
    {
        base.Awake();

        _playerInputHandler.Initiate();
    }

    protected override void Start()
    {
        base.Start();

        _playerInputHandler.Initialize();
    }
}
