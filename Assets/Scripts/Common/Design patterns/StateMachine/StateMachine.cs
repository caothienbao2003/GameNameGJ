using System;
using System.Collections.Generic;

public class StateMachine
{
    private IState _currentState;
    public IState CurrentState => _currentState;

    private readonly Dictionary<Type, List<StateTransition>> _transitions = new();
    private List<StateTransition> _currentTransitions = new();
    private static readonly List<StateTransition> EmptyTransitions = new(0);

    public void AddTransition(Type from, Type to, Func<bool> condition)
    {
        if (!_transitions.TryGetValue(from, out var transitions))
        {
            transitions = new List<StateTransition>();
            _transitions[from] = transitions;
        }
        transitions.Add(new StateTransition(to, condition));
    }

    public void Initialize(IState startingState)
    {
        _currentState = startingState;
        UpdateTransitionList();
        _currentState.Enter();
    }

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            ChangeState(transition.TargetType);

        _currentState?.Update();
    }

    public void FixedTick() => _currentState?.FixedUpdate();

    private void ChangeState(Type nextType)
    {
        // This logic is handled by the Component wrapper to find the instance
    }

    private StateTransition GetTransition()
    {
        foreach (var transition in _currentTransitions)
            if (transition.Condition()) return transition;
        return null;
    }

    public void SetStateDirectly(IState state)
    {
        _currentState?.Exit();
        _currentState = state;
        UpdateTransitionList();
        _currentState.Enter();
    }

    private void UpdateTransitionList()
    {
        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        _currentTransitions ??= EmptyTransitions;
    }
}