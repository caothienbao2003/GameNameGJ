using System;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private ActionCoordinator actionCoordinator;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private MoveToToDirectionPhysicsComponent moveHorizontalComponent;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private JumpPhysicsComponent jumpComponent;

    private MoveAction moveAction;
    private JumpAction jumpAction;

    private float _jumpBufferCounter;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private void Awake()
    {
        actionCoordinator = actionCoordinator ?? GetComponent<ActionCoordinator>();
        inputReader = inputReader ?? GetComponent<InputReader>();
        moveHorizontalComponent = moveHorizontalComponent ?? GetComponent<MoveToToDirectionPhysicsComponent>();
        rb = rb ?? GetComponent<Rigidbody2D>();
        jumpComponent = jumpComponent ?? GetComponent<JumpPhysicsComponent>();

        SetUpActionCoordinator();
    }

    private void SetUpActionCoordinator()
    {
        moveAction = new MoveAction(moveHorizontalComponent, inputReader, rb, 10);
        jumpAction = new JumpAction(jumpComponent, inputReader, rb, 10);
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        inputReader.OnJumpEvent += HandleJumpInput;
    }

    private void HandleJumpInput()
    {
        _jumpBufferCounter = jumpBufferTime;
    }


    void Update()
    {
        HandleMovementAction();
        HandleJumpAction();
    }

    private void HandleJumpAction()
    {
        if (_jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;

            // If the coordinator succeeds in starting the jump, clear the buffer
            if (actionCoordinator.TryStartAction(jumpAction))
            {
                _jumpBufferCounter = 0;
            }
        }
    }

    private void HandleMovementAction()
    {
        if (Mathf.Abs(inputReader.GetHorizontalMoveInput()) > 0.01f)
        {
            actionCoordinator.TryStartAction(moveAction);
        }
    }
}
