using System;
using UnityEngine;

public interface IPlayerInput
{
    event Action<float> OnMoveEvent;
    float GetHorizontalMoveInput();
    bool IsJumpPressed();
    event Action OnJumpEvent;
    event Action OnAttackEvent;
    event Action OnDashEvent;
    event Action OnFallEvent;
    float GetFallInput();
}