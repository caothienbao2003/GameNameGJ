using System.Collections.Generic;
using UnityEngine;

public class StateMachineComponent : MonoBehaviour
{
    private StateMachine _stateMachine;
    private Dictionary<System.Type, IState> _states = new();

    public void Initialize(IState startingState)
    {
        _stateMachine = new StateMachine();
        _stateMachine.Initialize(startingState);
    }

    public void AddState(IState state)
    {
        _states[state.GetType()] = state;
    }

    public void TransitionTo<T>() where T : IState
    {
        if (_states.TryGetValue(typeof(T), out var state))
        {
            _stateMachine.TransitionTo(state);
        }
    }

    private void Update() => _stateMachine?.Update();
    private void FixedUpdate() => _stateMachine?.FixedUpdate();
}