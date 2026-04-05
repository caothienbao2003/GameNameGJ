using UnityEngine;

public class MoveAction : IAction
{
    public int Priority { get; set; }
    private readonly IMoveToDirection _moveComponent;
    private readonly IPlayerInput _input;
    private readonly Rigidbody2D _rb;
    private readonly IAnimationService _animService;

    public MoveAction(IMoveToDirection motor, IPlayerInput input, Rigidbody2D rb, IAnimationService animService, int priority)
    {
        _moveComponent = motor;
        _input = input;
        _rb = rb;
        _animService = animService;
        Priority = priority;
    }

    public bool CanStart() => true;

    public void Start()
    {
        _animService.SetBool(AnimHash.IsMovingBool, true);
    }

    public void Update()
    {
        float xInput = _input.GetHorizontalMoveInput();
        float currentVelocity = Mathf.Abs(_rb.linearVelocity.x);
    }

    public void FixedUpdate()
    {
        float xInput = _input.GetHorizontalMoveInput();
        _moveComponent.MoveToDirection(new Vector3(xInput, 0, 0));
    }

    public bool IsFinished()
    {
        return Mathf.Abs(_input.GetHorizontalMoveInput()) <= 0.01f;
    }

    public void Stop()
    {
        _moveComponent.Stop();
        // Fallback: Ensure we transition to Idle when the action is forced to stop (e.g., Death/Cutscene)
        _animService.SetBool(AnimHash.IsMovingBool, false);
    }
}