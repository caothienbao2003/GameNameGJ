using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private ActionCoordinatorComponent actionCoordinator;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private MoveHorizontalComponent moveComponent;
    [SerializeField] private JumpPhysicsComponent jumpComponent;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private IAnimationService animService;
    [SerializeField] private PlayerVisuals playerVisuals;
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
        playerVisuals ??= GetComponentInChildren<PlayerVisuals>();
    }

    private void SetUpActions()
    {
        moveAction = new MoveAction(moveComponent, inputReader, rb, animService, 10);
        jumpAction = new JumpAction(jumpComponent, inputReader, rb, animService, 10);
    }

    private void OnEnable() => inputReader.OnJumpEvent += () => _jumpBufferCounter = jumpBufferTime;

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
    }
}