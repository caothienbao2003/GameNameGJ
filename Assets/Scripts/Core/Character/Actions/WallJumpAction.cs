using UnityEngine;

public class WallJumpAction : IAction
{
    public int Priority { get; set; }
    private readonly WallActionComponent _detector;
    private readonly Rigidbody2D _rb;
    private readonly IAnimationService _anim;
    private readonly FlipSprite _flip;
    
    private bool _isJumping;
    private float _timer;
    
    // This allows MoveAction to check if it should ignore input
    public bool IsInputLocked => _timer > 0; 

    public WallJumpAction(WallActionComponent detector, Rigidbody2D rb, IAnimationService anim, FlipSprite flip, int priority)
    {
        _detector = detector;
        _rb = rb;
        _anim = anim;
        _flip = flip;
        Priority = priority;
    }

    public bool CanStart() => _detector.IsTouchingWall(_flip.FaceDirection);

    public void Start()
    {
        _isJumping = true;
        // Celeste uses ~0.1s to 0.15s for the "lock"
        _timer = _detector.JumpDuration; 

        // 1. Reset velocity for a consistent jump feel
        _rb.linearVelocity = Vector2.zero;

        // 2. Jump AWAY from wall
        float jumpDir = -_flip.FaceDirection; 
        _rb.linearVelocity = new Vector2(jumpDir * _detector.JumpForce.x, _detector.JumpForce.y);
        
        _flip.Flip(jumpDir);
        _anim.Trigger(AnimHash.JumpTrigger);
    }

    public void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        
        // The action stays "Active" until we hit the ground or move 
        // but the 'IsFinished' logic depends on how you want to hand back control.
        if (_timer <= 0) _isJumping = false;
    }

    public bool IsFinished() => !_isJumping;

    public void Stop()
    {
        _isJumping = false;
        _anim.ReleasePriority();
    }

    public void FixedUpdate() { }
}