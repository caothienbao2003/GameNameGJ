using UnityEngine;

public class MoveAction : IAction
{
    public int Priority { get; set; }
    private readonly IMoveToDirection _motor;
    private readonly IPlayerInput _input;
    private readonly Rigidbody2D _rb;

    public MoveAction(IMoveToDirection motor, IPlayerInput input, Rigidbody2D rb, int priority)
    {
        _motor = motor;
        _input = input;
        _rb = rb;
        Priority = priority;
    }

    public bool CanStart() => true;
    public void Start() { }

    public void FixedUpdate()
    {
        float xInput = _input.GetHorizontalMoveInput();
        _motor.MoveToDirection(new Vector3(xInput, 0, 0));
    }

    public bool IsFinished()
    {
        // The action only ends when input is zero AND the character has physically stopped
        bool hasNoInput = Mathf.Abs(_input.GetHorizontalMoveInput()) < 0.01f;
        bool isStopped = Mathf.Abs(_rb.linearVelocity.x) < 0.01f;

        return hasNoInput && isStopped;
    }

    public void Stop()
    {
        // Final safety call to ensure velocity hits 0
        _motor.Stop();
    }

    public void Update() { }
}
