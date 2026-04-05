using UnityEngine;
public static class AnimHash
{
    public static readonly int IsMovingBool = Animator.StringToHash("isMoving");
    public static readonly int YVelocityFloat = Animator.StringToHash("yVelocity");
    public static readonly int JumpTrigger = Animator.StringToHash("Jump");
    public static readonly int IsGroundedBool = Animator.StringToHash("isGrounded");
    public static readonly int DashTrigger = Animator.StringToHash("DashStart");
    public static readonly int IsDashingBool = Animator.StringToHash("isDashing");
    public static readonly int IsWallSlidingBool = Animator.StringToHash("isWallSliding");
    public static readonly int WallClimbTrigger = Animator.StringToHash("WallClimb");
    public static readonly int WallLandTrigger = Animator.StringToHash("WallLand");

}