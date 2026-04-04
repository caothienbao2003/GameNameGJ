using UnityEngine;
public static class AnimHash
{
    public static readonly int IsMoving = Animator.StringToHash("isMoving");
    public static readonly int YVelocity = Animator.StringToHash("yVelocity");
    public static readonly int JumpTrigger = Animator.StringToHash("Jump");
    public static readonly int IsGrounded = Animator.StringToHash("isGrounded");
}