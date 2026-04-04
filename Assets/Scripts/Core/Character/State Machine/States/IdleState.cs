using System;
using UnityEngine;

public class IdleState : IState
{
    private readonly IPlayerInput _inputEvents;

    public IdleState(IPlayerInput inputEvents)
    {
        _inputEvents = inputEvents;
    }

    public void Enter()
    {
    }

    public void Exit()
    {

    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
    }

    private void HandleMoveInput(Vector2 vector)
    {
        
    }
}
