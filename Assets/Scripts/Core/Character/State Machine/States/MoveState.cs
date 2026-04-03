using UnityEngine;

public class MoveState : IState
{
    private readonly IMoveToDirection moveToDirection;
    public MoveState(IMoveToDirection moveToDirection)
    {
        this.moveToDirection = moveToDirection;
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
}
