using UnityEngine;

public class WallSlideAction : IAction
{
    public int Priority { get; set; }
    private readonly WallActionComponent _detector;
    private readonly IPlayerInput _input;
    private readonly Rigidbody2D _rb;
    private readonly IAnimationService _anim;
    private readonly FlipSprite _flip;
    private readonly IJumpComponent _jumpComponent;

    // private float _slideSpeed = -2f;
    private bool _isSliding;

    public WallSlideAction(WallActionComponent wallActionComponent, IPlayerInput input, Rigidbody2D rb, IAnimationService anim, FlipSprite flip, IJumpComponent jumpComponent, int priority)
    {
        _detector = wallActionComponent;
        _input = input;
        _rb = rb;
        _anim = anim;
        _flip = flip;
        _jumpComponent = jumpComponent;
        Priority = priority;
    }

    public bool CanStart()
    {
        float moveInput = _input.GetHorizontalMoveInput();
        // We remove the Y velocity check here so the Action Coordinator 
        // doesn't immediately kill the action if velocity fluctuates
        return _detector.IsTouchingWall(_flip.FaceDirection) &&
               Mathf.Abs(moveInput) > 0.1f && !_jumpComponent.IsGrounded();
    }

    public void Start()
    {
        _isSliding = true;
        _anim.SetBool(AnimHash.IsWallSlidingBool, true);
        _anim.Trigger(AnimHash.WallLandTrigger);
    }

    public void Update()
    {
        if (!CanStart()) _isSliding = false;
    }

    public void FixedUpdate()
    {
        if (_isSliding)
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _detector.SlideSpeed);
    }

    public bool IsFinished() => !_isSliding;

    public void Stop()
    {
        _isSliding = false;
        _anim.SetBool(AnimHash.IsWallSlidingBool, false);
        _anim.ReleasePriority();
    }
}