using UnityEngine;

public class DashAction : IAction
{
    public int Priority { get; set; }
    private readonly IDashComponent _dashMotor;
    private readonly IPlayerInput _input;
    private readonly IAnimationService _anim;
    private readonly FlipSprite _flipSprite;
    private bool _isDashing;

    // Add 'Transform playerTransform' to the constructor
    public DashAction(IDashComponent motor, IPlayerInput input, IAnimationService anim, FlipSprite flipSprite, int priority)
    {
        _dashMotor = motor;
        _input = input;
        _anim = anim;
        _flipSprite = flipSprite;
        Priority = priority;
    }

    public bool CanStart() => true;

    public void Start()
    {
        _isDashing = true;

        // Calculate direction from input
        Vector2 inputDir = new Vector2(_input.GetHorizontalMoveInput(), _input.GetVerticalMoveInput());

        Vector2 dashDir;
        if (inputDir.sqrMagnitude > 0.01f)
        {
            dashDir = inputDir.normalized;
        }
        else
        {
            // If no input, dash in the direction the player is currently facing
            // Assuming localScale.x > 0 is Right and < 0 is Left
            float facing = _flipSprite.FaceDirection;
            dashDir = new Vector2(facing, 0);
        }

        _anim.PlayForce(AnimHash.DashTrigger, 20);

        _dashMotor.ExecuteDash(dashDir, () =>
        {
            _isDashing = false;
            _anim.ReleasePriority();
        });

        _anim.SetBool(AnimHash.IsDashingBool, true);
    }

    public void Update() { }
    public void FixedUpdate() { }
    public bool IsFinished() => !_isDashing;
    public void Stop()
    {
        _isDashing = false;
        _anim.SetBool(AnimHash.IsDashingBool, false);
    }
}