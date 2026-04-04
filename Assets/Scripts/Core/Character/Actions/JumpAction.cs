using UnityEngine;

public class JumpAction : IAction
{
    public int Priority { get; set; }
    private readonly IJumpComponent _jumpMotor;
    private readonly IPlayerInput _input;
    private readonly Rigidbody2D _rb;
    private bool _isJumping;

    public JumpAction(IJumpComponent jumpComponent, IPlayerInput input, Rigidbody2D rb, int priority)
    {
        _jumpMotor = jumpComponent;
        _input = input;
        _rb = rb;
        Priority = priority;
    }

    public bool CanStart() => _jumpMotor.IsCoyoteTimeActive();

    public void Start()
    {
        _isJumping = true;
        _jumpMotor.ApplyJumpImpulse();
        _jumpMotor.ConsumeCoyoteTime();
    }

    public void FixedUpdate()
    {
        if (_isJumping && !_input.IsJumpPressed() && _rb.linearVelocity.y > 0)
        {
            _jumpMotor.CutJumpVelocity();
            _isJumping = false;
        }
    }

    public void Update()
    {
        float yVel = _rb.linearVelocity.y;
        float fallInput = _input.GetFallInput();

        if (_jumpMotor.IsGrounded())
        {
            _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity);
            if (yVel <= 0.1f) _isJumping = false;
        }
        else
        {
            // 1. Down Press (Celeste Fast Fall)
            if (fallInput < -0.5f)
                _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity * _jumpMotor.DownPressMult);
            // 2. Apex (Hollow Knight Float)
            else if (Mathf.Abs(yVel) < _jumpMotor.JumpHangThreshold)
                _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity * _jumpMotor.JumpHangGravityMult);
            // 3. Falling
            else if (yVel < 0)
                _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity * _jumpMotor.FallGravityMult);
            // 4. Rising
            else
                _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity);
        }
    }

    public bool IsFinished() => !_isJumping && _jumpMotor.IsGrounded() && _rb.linearVelocity.y <= 0.1f;

    public void Stop()
    {
        _isJumping = false;
        _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity);
    }
}