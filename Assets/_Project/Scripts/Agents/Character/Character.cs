using UnityEngine;
using static CharactersDatabase;

public class Character : MonoBehaviour
{
    public int ID { get; protected set; }
    public Rigidbody2D Body { get; protected set; }
    public CharacterModel Model { get; protected set; }
    public CharacterMovementHandler MovementHandler { get; protected set; }
    public PlayerInputHandler InputHandler { get; protected set; }

    public virtual void Setup(int p_id, CharacterData p_data, InputListener p_listener)
    {
        Body = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Model = GetComponentInChildren<CharacterModel>();
        MovementHandler = GetComponent<CharacterMovementHandler>();

        ID = p_id;
        InputHandler.Setup(p_listener);
        Model.Setup(p_data);
        MovementHandler.Setup(Body);

        gameObject.SetActive(true);
    }

    protected virtual void Awake()
    {
        InputHandler.onMovementPerformed += InputHandler_onMovementPerformed;

        InputHandler.Intiate();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        MovementHandler.Tick();
    }

    protected virtual void FixedUpdate()
    {
        MovementHandler.FixedTick();
    }

    private void InputHandler_onMovementPerformed(Vector2 p_direction)
    {
        Debug.Log("InputHandler_onMovementPerformed");
        MovementHandler.SetDirection(p_direction);
    }
}
