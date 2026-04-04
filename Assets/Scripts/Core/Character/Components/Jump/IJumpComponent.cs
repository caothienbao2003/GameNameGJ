using System;

public interface IJumpComponent
{
    float DefaultGravity { get; }
    float FallGravityMult { get; }
    float JumpHangGravityMult { get; }
    float JumpHangThreshold { get; }
    float DownPressMult { get; }
    bool IsGrounded();
    bool IsCoyoteTimeActive();
    void ConsumeCoyoteTime();
    void ApplyJumpImpulse();
    void CutJumpVelocity();
    void SetGravityScale(float scale);
    event Action OnJumpEvent;
    event Action OnLandEvent;
}