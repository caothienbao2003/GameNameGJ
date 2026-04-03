using UnityEngine;

public class MoveAction : IAction
{
    public int Priority {get; set;}
    private IMoveToDirection moveToDirection;
    private IInputEvents inputEvents;
    private Rigidbody2D rb;
    public MoveAction(IMoveToDirection moveToDirection, IInputEvents inputEvents, Rigidbody2D rb, int priority)
    {
        this.moveToDirection = moveToDirection;
        this.inputEvents = inputEvents;
        this.rb = rb;
        Priority = priority;
    }

    public bool CanStart() => true;

    public void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(inputEvents.GetHorizontalMoveInput(), 0f, 0f);
        moveToDirection.MoveToDirection(moveDirection);
    }

    public void Start()
    {
    }

    public void Stop()
    {
        moveToDirection.Stop();
    }

    public void Update()
    {
    }
}
