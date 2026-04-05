using UnityEngine;

public class IdleAction : IAction
{
    public int Priority { get; set; }
    private readonly IMoveToDirection _moveComponent;
    private readonly IAnimationService _anim;

    public IdleAction(IMoveToDirection motor, IAnimationService anim, int priority)
    {
        _moveComponent = motor;
        _anim = anim;
        Priority = priority;
    }

    public bool CanStart() => true;

    public void Start()
    {
    }

    public void Update() { }

    public void FixedUpdate()
    {
        // Tell the motor to bring us to a halt using its deceleration logic
        _moveComponent.Stop(); 
    }

    public bool IsFinished() => false; // Idle is the ultimate fallback

    public void Stop() { }
}