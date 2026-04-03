using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInputActions.IGameplayActions, IInputEvents
{
    public event Action<float> OnMoveEvent;
    private float _horizontalMove;
    public float GetHorizontalMoveInput() => _horizontalMove;
    public event Action OnJumpEvent;
    public event Action OnAttackEvent;
    public event Action OnDashEvent;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Gameplay.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerInputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Gameplay.Disable();
    }

    public void OnHorizontalMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _horizontalMove = context.ReadValue<float>();
            OnMoveEvent?.Invoke(_horizontalMove);
        }
        else if (context.canceled)
        {
            _horizontalMove = 0f;
            OnMoveEvent?.Invoke(_horizontalMove);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackEvent?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDashEvent?.Invoke();
    }
}