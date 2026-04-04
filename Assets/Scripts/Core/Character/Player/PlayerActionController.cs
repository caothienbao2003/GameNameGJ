using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private ActionCoordinator actionCoordinator;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private MoveToToDirectionPhysicsComponent moveComponent;
    [SerializeField] private JumpPhysicsComponent jumpComponent;
    [SerializeField] private Rigidbody2D rb;

    private MoveAction moveAction;
    private JumpAction jumpAction;

    [SerializeField] private float jumpBufferTime = 0.12f;
    private float _jumpBufferCounter;

    private void Awake()
    {
        actionCoordinator = actionCoordinator ?? GetComponent<ActionCoordinator>();
        inputReader = inputReader ?? GetComponent<InputReader>();
        moveComponent = moveComponent ?? GetComponent<MoveToToDirectionPhysicsComponent>();
        jumpComponent = jumpComponent ?? GetComponent<JumpPhysicsComponent>();
        rb = rb ?? GetComponent<Rigidbody2D>();

        moveAction = new MoveAction(moveComponent, inputReader, rb, 10);
        jumpAction = new JumpAction(jumpComponent, inputReader, rb, 10);
    }

    private void OnEnable() => inputReader.OnJumpEvent += () => _jumpBufferCounter = jumpBufferTime;

    void Update()
    {
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