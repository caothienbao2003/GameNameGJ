using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }
    public System.Action<IState> OnStateChanged;

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
        OnStateChanged?.Invoke(CurrentState);
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
        OnStateChanged?.Invoke(CurrentState);
    }

    public void Update() => CurrentState?.Update();
    public void FixedUpdate() => CurrentState?.FixedUpdate();
}