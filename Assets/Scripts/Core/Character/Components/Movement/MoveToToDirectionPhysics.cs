using System;
using UnityEngine;

public class MoveToToDirectionPhysicsComponent : MonoBehaviour, IMoveToDirection
{
    private enum MoveType
    {
        Velocity,
        AddForce
    }
    [SerializeField] private MoveType moveType = MoveType.Velocity;

    [SerializeField] private float moveSpeed;

    private Vector3 moveDirection;

    private Rigidbody2D _rigidbody2D;

    private Rigidbody2D rb => _rigidbody2D ??= GetComponent<Rigidbody2D>();

    public void MoveToDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
        this.moveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        if (moveDirection == Vector3.zero) return;
        switch (moveType)
        {
            case MoveType.Velocity:
                rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocityY, 0f);
                break;
            case MoveType.AddForce:
                rb.AddForce(moveDirection * moveSpeed, ForceMode2D.Force);
                break;
        }

    }

    public void Stop()
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocityY, 0f);
    }
}
