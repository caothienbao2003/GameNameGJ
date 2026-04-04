using UnityEngine;

public interface IJumpComponent
{
    bool IsGrounded();
    void ApplyJumpImpulse();
    void CutJumpVelocity();
    void SetGravityScale(float scale);
    float DefaultGravity { get; }
    float FallGravityMult { get; }
    float JumpHangGravityMult { get; }
    float JumpHangThreshold { get; }
    float DownPressMult { get; }
}