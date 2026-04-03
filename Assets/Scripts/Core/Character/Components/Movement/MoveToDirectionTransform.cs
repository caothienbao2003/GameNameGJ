using System;
using UnityEngine;

public class MoveToDirectionTransform : MonoBehaviour, IMoveToDirection
{
    [SerializeField] private float moveSpeed;

    private Vector3 moveDirection;

    // public bool IsMoving()
    // {
    //     return moveDirection != Vector3.zero;
    // }

    // public void SetMoveDirection(Vector3 moveDirection)
    // {
    //     Debug.Log($"[MovableTransform] SetMoveDirection] {moveDirection}");
    //     this.moveDirection = moveDirection;
    //     moveDirection.Normalize();
    // }

    // private void Update()
    // {
    //     if (!IsMoving()) return;
    //     transform.position += moveDirection * (moveSpeed * Time.deltaTime);
    // }

    public void MoveToDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        
        transform.position += direction.normalized * (moveSpeed * Time.deltaTime);
        Debug.Log($"Moving: {direction}");
    }

    public void Stop()
    {
        
    }
}