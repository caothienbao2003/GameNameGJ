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
    
    public void Start() { /* Animation handled in Update */ }

    public void Update() 
    {
        float xInput = _input.GetHorizontalMoveInput();
        float currentVelocity = Mathf.Abs(_rb.linearVelocity.x);

        if (Mathf.Abs(xInput) > 0.01f || currentVelocity > 0.1f)
        {
           _animService.SetBool(AnimHash.IsMovingBool, true);
        }
        else
        {
            // Otherwise, this action is responsible for Idle
            _animService.SetBool(AnimHash.IsMovingBool, false);
        }
    }

    public void FixedUpdate()
    {
        float xInput = _input.GetHorizontalMoveInput();
        _moveComponent.MoveToDirection(new Vector3(xInput, 0, 0));
    }

    public bool IsFinished()
    {
        // For a platformer/top-down, Move is usually the "Base Action".
        // It's often better to return false so the ActionCoordinator 
        // doesn't constantly kill/revive the move logic.
        return false; 
    }

    public void Stop()
    {
        _moveComponent.Stop();
        // Fallback: Ensure we transition to Idle when the action is forced to stop (e.g., Death/Cutscene)
        _animService.SetBool(AnimHash.IsMovingBool, false);
    }
}