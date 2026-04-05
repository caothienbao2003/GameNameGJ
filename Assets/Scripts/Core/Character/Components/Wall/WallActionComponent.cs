using UnityEngine;

public class WallActionComponent : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private Transform headPosition; // Positioned near the head

    [Header("Wall Slide")]
    public float SlideSpeed = -2f;

    [Header("Wall Jump")]
    public Vector2 JumpForce = new Vector2(10f, 15f);
    public float JumpDuration = 0.2f;

    [Header("Ledge Climb")]
    public Vector2 ClimbOffset = new Vector2(0.8f, 1.2f);
    public float ClimbDuration = 0.4f;
    public bool IsTouchingWall(float facingDir) 
    {
        return Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, wallLayer);
    }

    public bool CanLedgeClimb(float facingDir)
    {
        // Touching wall but head-point is NOT touching wall = Ledge found!
        bool bodyTouching = IsTouchingWall(facingDir);
        bool headTouching = Physics2D.Raycast(headPosition.position, Vector2.right * facingDir, wallCheckDistance, wallLayer);
        return bodyTouching && !headTouching;
    }
}