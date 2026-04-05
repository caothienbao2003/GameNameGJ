using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInputActions.IGameplayActions, IPlayerInput
{
    public event Action<float> OnMoveEvent;
    private float _horizontalMove;
    public float GetHorizontalMoveInput() => _horizontalMove;
    private bool _isJumpPressed;
    public bool IsJumpPressed() => _isJumpPressed;
    public event Action OnJumpEvent;
    public event Action OnAttackEvent;
    public event Action OnDashEvent;
    public event Action OnFallEvent;
    private float _fallInputValue;
    public float GetFallInput() => _fallInputValue;
    private float _verticalMove;
    public float GetVerticalMoveInput() => _verticalMove;

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
        {
            _isJumpPressed = true;
            OnJumpEvent?.Invoke();
        }
        else if (context.canceled)
        {
            _isJumpPressed = false;
        }

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

    public void OnFall(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _fallInputValue = -context.ReadValue<float>();
            Debug.Log($"Fall input value: {_fallInputValue}");
            OnFallEvent?.Invoke();
        }
        else if (context.canceled)
        {
            _fallInputValue = 0f;
            OnFallEvent?.Invoke();
        }
    }

    public void OnVerticalMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _verticalMove = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            _verticalMove = 0f;
        }
    }
}