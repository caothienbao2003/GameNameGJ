using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineComponent : MonoBehaviour, IStateMachineComponent
{
    private StateMachine _stateMachine = new();
    private readonly Dictionary<Type, IState> _stateInstances = new();

    public void AddState(IState state) => _stateInstances[state.GetType()] = state;

    public void AddTransition<TTransitionFrom, TTo>(Func<bool> condition) 
        where TTransitionFrom : IState where TTo : IState
    {
        _stateMachine.AddTransition(typeof(TTransitionFrom), typeof(TTo), condition);
    }

    public void Initialize(IState startingState)
    {
        if (!_stateInstances.ContainsKey(startingState.GetType()))
            AddState(startingState);
            
        _stateMachine.Initialize(startingState);
    }

    private void Update()
    {
        // Check for transitions and update logic
        _stateMachine.Tick();

        // Check if the machine requested a state change
        // (Handled internally via the Tick's transition check)
    }

    // This helper allows the StateMachine to request the instance from the Component
    public void TransitionTo<T>() where T : IState
    {
        if (_stateInstances.TryGetValue(typeof(T), out var state))
            _stateMachine.SetStateDirectly(state);
    }

    private void FixedUpdate() => _stateMachine.FixedTick();
}