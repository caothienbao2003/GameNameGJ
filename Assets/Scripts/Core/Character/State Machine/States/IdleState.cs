using System;
using UnityEngine;

public class IdleState : IState
{
    private readonly IInputEvents _inputEvents;

    public IdleState(IInputEvents inputEvents)
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
