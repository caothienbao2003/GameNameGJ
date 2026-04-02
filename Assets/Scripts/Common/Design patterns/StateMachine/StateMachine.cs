using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }

    // Event for UI or VFX to listen to when states change
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