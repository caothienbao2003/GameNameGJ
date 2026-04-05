using UnityEngine;

public class JumpAction : IAction
{
    public int Priority { get; set; }
    private readonly IJumpComponent _jumpComponent;
    private readonly IPlayerInput _input;
    private readonly Rigidbody2D _rb;
    private readonly IAnimationService _animService;
    private readonly IMoveToDirection _moveComponent;
    private bool _isJumping;

    public JumpAction(IJumpComponent jumpComponent, IPlayerInput input, Rigidbody2D rb, IAnimationService animService, IMoveToDirection moveComponent, int priority)
    {
        _jumpComponent = jumpComponent;
        _input = input;
        _rb = rb;
        _animService = animService;
        _moveComponent = moveComponent;
        Priority = priority;
    }

    public bool CanStart() => _jumpComponent.IsCoyoteTimeActive();

    public void Start()
    {
        _isJumping = true;
        _jumpComponent.ApplyJumpImpulse();
        _jumpComponent.ConsumeCoyoteTime();

        _animService.Trigger(AnimHash.JumpTrigger);
    }

    public void FixedUpdate()
    {
        if (_isJumping && !_input.IsJumpPressed() && _rb.linearVelocity.y > 0)
        {
            _jumpComponent.CutJumpVelocity();
            _isJumping = false;
        }
    }

    public void Update()
    {
        float yVel = _rb.linearVelocity.y;
        float fallInput = _input.GetFallInput();

        if (_jumpComponent.IsGrounded())
        {
            if (!_isJumping && yVel < -0.1f)
                _jumpComponent.SetGravityScale(_jumpComponent.DefaultGravity);
            if (yVel <= 0.1f) _isJumping = false;
        }
        else
        {
            // 1. Down Press (Celeste Fast Fall)
            if (fallInput < -0.5f)
                _jumpComponent.SetGravityScale(_jumpComponent.DefaultGravity * _jumpComponent.DownPressMult);
            // 2. Apex (Hollow Knight Float)
            else if (Mathf.Abs(yVel) < _jumpComponent.JumpHangThreshold)
                _jumpComponent.SetGravityScale(_jumpComponent.DefaultGravity * _jumpComponent.JumpHangGravityMult);
            // 3. Falling
            else if (yVel < 0)
            {
                _jumpComponent.SetGravityScale(_jumpComponent.DefaultGravity * _jumpComponent.FallGravityMult);
            }
            // 4. Rising
            else
                _jumpComponent.SetGravityScale(_jumpComponent.DefaultGravity);
        }
    }

    public bool IsFinished() => !_isJumping && _jumpComponent.IsGrounded() && _rb.linearVelocity.y <= 0.1f;

    public void Stop()
    {
        _isJumping = false;
        _jumpComponent.SetGravityScale(_jumpComponent.DefaultGravity);
    }
}