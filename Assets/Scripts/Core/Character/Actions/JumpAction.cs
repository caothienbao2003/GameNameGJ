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

    public bool CanStart() => _jumpMotor.IsGrounded();

    public void Start()
    {
        _isJumping = true;
        _jumpMotor.ApplyJumpImpulse();
    }

    public void FixedUpdate()
    {
        // VARIABLE JUMP HEIGHT LOGIC
        // If the player releases the button early while moving up, cut the jump short
        if (_isJumping && !_input.IsJumpPressed())
        {
            _jumpMotor.CutJumpVelocity();
            _isJumping = false; // Prevents cutting multiple times in one jump
        }
    }

    public void Update()
    {
        float yVel = _rb.linearVelocity.y;
        float verticalInput = _input.GetFallInput();

        // 1. FAST FALL (Holding Down)
        // If we are in the air and the player is pressing Down (usually < 0)
        if (!_jumpMotor.IsGrounded() && verticalInput < -0.1f)
        {
            _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity * _jumpMotor.DownPressMult);
        }
        // 2. THE APEX (Slow in Air)
        else if (!_jumpMotor.IsGrounded() && Mathf.Abs(yVel) < _jumpMotor.JumpHangThreshold)
        {
            _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity * _jumpMotor.JumpHangGravityMult);
        }
        // 3. NORMAL DESCENT (Standard Fall)
        else if (yVel < 0)
        {
            _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity * _jumpMotor.FallGravityMult);
        }
        // 4. THE RISE
        else
        {
            _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity);
        }

        // Standard Landing Logic
        if (_isJumping && _jumpMotor.IsGrounded() && yVel <= 0.1f)
        {
            _isJumping = false;
        }
    }

    public bool IsFinished()
    {
        // A jump is finished when we are back on the ground 
        // AND we are no longer in the initial "Start" phase (falling/landing)
        return !_isJumping && _jumpMotor.IsGrounded() && _rb.linearVelocity.y <= 0.1f;
    }

    public void Stop()
    {
        _isJumping = false;
        // Safety: Reset gravity when action is interrupted
        _jumpMotor.SetGravityScale(_jumpMotor.DefaultGravity);
    }
}