using UnityEngine;
using DG.Tweening; // Ensure DOTween is imported

public class WallEdgeClimbAction : IAction
{
    public int Priority { get; set; }
    private readonly WallActionComponent _wallActionComponent;
    private readonly Rigidbody2D _rb;
    private readonly IAnimationService _anim;
    private readonly FlipSprite _flip;

    private bool _isClimbing;
    // private Vector2 _climbOffset = new Vector2(1.2f, 1.2f);
    // private float _climbDuration = 0.4f; // Adjusted for a smoother tween

    public WallEdgeClimbAction(WallActionComponent wallActionComponent, Rigidbody2D rb, IAnimationService anim, FlipSprite flip, int priority)
    {
        _wallActionComponent = wallActionComponent;
        _rb = rb;
        _anim = anim;
        _flip = flip;
        Priority = priority;
    }

    public bool CanStart() => _wallActionComponent.CanLedgeClimb(_flip.FaceDirection);

    public void Start()
    {
        _isClimbing = true;
        _rb.simulated = false; // Physics is off
        _rb.linearVelocity = Vector2.zero; // Kill momentum
        _anim.Trigger(AnimHash.WallClimbTrigger);

        Vector2 finishPos = _rb.position + new Vector2(_flip.FaceDirection * _wallActionComponent.ClimbOffset.x, _wallActionComponent.ClimbOffset.y);

        // Use transform instead of _rb while simulated is false
        _rb.transform.DOMove(finishPos, _wallActionComponent.ClimbDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(Stop);
    }

    public void Update() { /* Managed by DOTween */ }

    public void FixedUpdate() { /* Managed by DOTween */ }

    public bool IsFinished() => !_isClimbing;

    public void Stop()
    {
        if (!_isClimbing) return;

        // Kill any active tweens on this object to prevent "ghost" movement
        _rb.DOKill();

        _isClimbing = false;
        _rb.simulated = true;
        _rb.linearVelocity = Vector2.zero;
        _anim.ReleasePriority();
    }
}