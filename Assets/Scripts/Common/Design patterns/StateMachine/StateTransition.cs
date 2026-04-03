using System;

public class StateTransition
{
    public Type TargetType { get; }
    public Func<bool> Condition { get; }

    public StateTransition(Type targetType, Func<bool> condition)
    {
        TargetType = targetType;
        Condition = condition;
    }
}