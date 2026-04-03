using System;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private ActionCoordinator actionCoordinator;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private MoveToToDirectionPhysicsComponent moveToToDirectionPhysics;
    [SerializeField] private Rigidbody2D rb;

    private MoveAction moveAction;

    private void Awake()
    {
        actionCoordinator = actionCoordinator ?? GetComponent<ActionCoordinator>();
        inputReader = inputReader ?? GetComponent<InputReader>();
        moveToToDirectionPhysics = moveToToDirectionPhysics ?? GetComponent<MoveToToDirectionPhysicsComponent>();
        rb = rb ?? GetComponent<Rigidbody2D>();

        SetUpActionCoordinator();
    }

    private void SetUpActionCoordinator()
    {
        moveAction = new MoveAction(moveToToDirectionPhysics, inputReader, rb, 10);
    }

    void Update()
    {
        HandleMovementAction();
    }

    private void HandleMovementAction()
    {
        if (inputReader.GetHorizontalMoveInput() != 0)
        {
            actionCoordinator.TryStartAction(moveAction);
        }
        else
        {
            actionCoordinator.TryStopAction(moveAction);
        }
    }
}
