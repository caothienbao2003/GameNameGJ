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
    private MoveAction moveAction;
    private JumpAction jumpAction;

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
        rb ??= GetComponent<Rigidbody2D>();
        animService ??= GetComponent<AnimatorComponent>();

        flipSpriteComponent ??= GetComponentInChildren<FlipSprite>();
        stretchSpriteComponent ??= GetComponentInChildren<StretchSprite>();
    }

    private void SetUpActions()
    {
        moveAction = new MoveAction(moveComponent, inputReader, rb, animService, 10);
        jumpAction = new JumpAction(jumpComponent, inputReader, rb, animService, 10);
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
        jumpComponent.OnJumpEvent += stretchSpriteComponent.PlayJumpEffect;
        jumpComponent.OnLandEvent += stretchSpriteComponent.PlayLandEffect;
    }

    private void UnRegisterEvents()
    {
        inputReader.OnJumpEvent -= () => _jumpBufferCounter = jumpBufferTime;
        jumpComponent.OnJumpEvent -= stretchSpriteComponent.PlayJumpEffect;
        jumpComponent.OnLandEvent -= stretchSpriteComponent.PlayLandEffect;
    }

    void Update()
    {
        animService.SetBool(AnimHash.IsGrounded, jumpComponent.IsGrounded());
        animService.SetFloat(AnimHash.YVelocity, rb.linearVelocityY);

        // Handle Movement
        if (Mathf.Abs(inputReader.GetHorizontalMoveInput()) > 0.01f)
            actionCoordinator.TryStartAction(moveAction);

        // Handle Jump Buffer
        if (_jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
            if (actionCoordinator.TryStartAction(jumpAction)) _jumpBufferCounter = 0;
        }

        HandleSpriteFlip();
        HandleSpriteDynamicStretch();
    }

    private void HandleSpriteDynamicStretch()
    {
        stretchSpriteComponent.HandleDynamicStretch(rb.linearVelocity.y);
    }

    private void HandleSpriteFlip()
    {
        flipSpriteComponent.Flip(inputReader.GetHorizontalMoveInput());
    }
}