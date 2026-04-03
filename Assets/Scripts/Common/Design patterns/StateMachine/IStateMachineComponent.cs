using System;

public interface IStateMachineComponent
{
    void AddState(IState state);
    void AddTransition<TFrom, TTo>(Func<bool> condition) where TFrom : IState where TTo : IState;
    void Initialize(IState startingState);
}
