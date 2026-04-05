using System;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private ActionCoordinatorComponent actionCoordinator;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private MoveHorizontalComponent moveComponent;
    [SerializeField] private JumpPhysicsComponent jumpComponent;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private IAnimationService animService;
    [SerializeField] private FlipSprite flipSpriteComponent;
    [SerializeField] private StretchSprite stretchSpriteComponent;
    [SerializeField] private DashComponent dashComponent;
    [SerializeField] private WallActionComponent wallDetectionComponent;
    private IdleAction idleAction;
    private MoveAction moveAction;
    private JumpAction jumpAction;
    private DashAction dashAction;
    private WallJumpAction wallJumpAction;
    private WallSlideAction wallSlideAction;
    private WallEdgeClimbAction wallEdgeClimbAction;

    [SerializeField] private float jumpBufferTime = 0.12f;
    private float _jumpBufferCounter;

    private void Awake()
    {
        RegisterComponents();

        SetUpActions();
    }

    private void RegisterComponents()
    {
        actionCoordinator ??= GetComponent<ActionCoordinatorComponent>();
        inputReader ??= GetComponent<InputReader>();
        moveComponent ??= GetComponent<MoveHorizontalComponent>();
        jumpComponent ??= GetComponent<JumpPhysicsComponent>();
        wallDetectionComponent ??= GetComponent<WallActionComponent>();
        rb ??= GetComponent<Rigidbody2D>();
        animService ??= GetComponent<AnimatorComponent>();
        dashComponent ??= GetComponent<DashComponent>();

        flipSpriteComponent ??= GetComponentInChildren<FlipSprite>();
        stretchSpriteComponent ??= GetComponentInChildren<StretchSprite>();
    }

    private void SetUpActions()
    {
        idleAction = new IdleAction(moveComponent, animService, 0);
        moveAction = new MoveAction(moveComponent, inputReader, rb, animService, 10);
        jumpAction = new JumpAction(jumpComponent, inputReader, rb, animService, moveComponent, 10);
        dashAction = new DashAction(dashComponent, inputReader, animService, flipSpriteComponent, 20);

        wallJumpAction = new WallJumpAction(wallDetectionComponent, rb, animService, flipSpriteComponent, 30);
        wallSlideAction = new WallSlideAction(wallDetectionComponent, inputReader, rb, animService, flipSpriteComponent, jumpComponent, 25);
        wallEdgeClimbAction = new WallEdgeClimbAction(wallDetectionComponent, rb, animService, flipSpriteComponent, 40);
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void RegisterEvents()
    {
        inputReader.OnJumpEvent += () => _jumpBufferCounter = jumpBufferTime;
        inputReader.OnDashEvent += () => actionCoordinator.TryStartAction(dashAction);
        if (jumpComponent != null && stretchSpriteComponent != null)
        {
            jumpComponent.OnJumpEvent += stretchSpriteComponent.PlayJumpEffect;
            jumpComponent.OnLandEvent += stretchSpriteComponent.PlayLandEffect;
        }
    }

    private void UnRegisterEvents()
    {
        inputReader.OnJumpEvent -= () => _jumpBufferCounter = jumpBufferTime;
        inputReader.OnDashEvent -= () => actionCoordinator.TryStartAction(dashAction);

        if (jumpComponent != null && stretchSpriteComponent != null)
        {
            jumpComponent.OnJumpEvent -= stretchSpriteComponent.PlayJumpEffect;
            jumpComponent.OnLandEvent -= stretchSpriteComponent.PlayLandEffect;
        }
    }

    void Update()
    {
        animService.SetBool(AnimHash.IsGroundedBool, jumpComponent.IsGrounded());
        animService.SetFloat(AnimHash.YVelocityFloat, rb.linearVelocityY);

        actionCoordinator.TryStartAction(idleAction); // Idle is the fallback if no other actions are active

        // Handle Movement
        if (Mathf.Abs(inputReader.GetHorizontalMoveInput()) > 0.01f)
        {
            actionCoordinator.TryStartAction(moveAction);
        }

        // Handle Jump Buffer
        if (_jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
            if (actionCoordinator.TryStartAction(jumpAction)) _jumpBufferCounter = 0;
        }

        // 1. Always check for Wall Climb first (Highest Priority)
        if (wallDetectionComponent.CanLedgeClimb(flipSpriteComponent.FaceDirection))
        {
            actionCoordinator.TryStartAction(wallEdgeClimbAction);
        }

        // 2. Check Wall Interactions
        else if (wallDetectionComponent.IsTouchingWall(flipSpriteComponent.FaceDirection))
        {
            // WALL JUMP: Check buffer first
            if (_jumpBufferCounter > 0)
            {
                if (actionCoordinator.TryStartAction(wallJumpAction))
                {
                    _jumpBufferCounter = 0;
                    return;
                }
            }

            // WALL SLIDE: Trigger automatically when falling/touching wall
            // We removed 'isPushingWall' here.
            if (rb.linearVelocity.y <= 0.1f)
            {
                actionCoordinator.TryStartAction(wallSlideAction);
            }
        }

        // // 2. Check Wall Interactions
        // else if (wallDetectionComponent.IsTouchingWall(flipSpriteComponent.FaceDirection))
        // {
        //     float horizontalInput = inputReader.GetHorizontalMoveInput();
        //     bool isPushingWall = horizontalInput * flipSpriteComponent.FaceDirection > 0.1f;

        //     // WALL JUMP: Only if the buffer is active (recent press)
        //     if (_jumpBufferCounter > 0)
        //     {
        //         if (actionCoordinator.TryStartAction(wallJumpAction))
        //         {
        //             _jumpBufferCounter = 0; // Consume the buffer
        //             return; // Exit so we don't slide on the same frame
        //         }
        //     }

        //     // WALL SLIDE: Only if falling AND pushing into the wall
        //     if (rb.linearVelocity.y <= 0 && isPushingWall)
        //     {
        //         actionCoordinator.TryStartAction(wallSlideAction);
        //     }
        // }

        HandleSpriteFlip();
        HandleSpriteDynamicStretch();
    }

    private void HandleSpriteDynamicStretch()
    {
        if (stretchSpriteComponent != null)
        {
            stretchSpriteComponent.HandleDynamicStretch(rb.linearVelocity.y);
        }
    }

    private void HandleSpriteFlip()
    {
        flipSpriteComponent.Flip(inputReader.GetHorizontalMoveInput());
    }
}