using System;
using UnityEngine;

public interface IInputEvents
{
    event Action<float> OnMoveEvent;
    float GetHorizontalMoveInput();
    event Action OnJumpEvent;
    event Action OnAttackEvent;
    event Action OnDashEvent;
}