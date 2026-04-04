using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpPhysicsComponent : MonoBehaviour, IJumpComponent
{
    [Header("Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f);

    [Header("Base Physics")]
    [SerializeField] private float jumpForce = 16f; // "Jump Fast" starts here
    [SerializeField] private float defaultGravity = 4f;
    [SerializeField] private float jumpCutMultiplier = 0.3f;

    [Header("The 'Feel' Multipliers")]
    [SerializeField] private float fallGravityMultiplier = 1.9f; // "Fall Fast"
    [SerializeField] private float hangGravityMultiplier = 0.5f; // "Slow in Air"
    [SerializeField] private float hangThreshold = 2.0f;         // Velocity range for the peak

    [Header("Fall Settings")]
    [SerializeField] private float downPressGravityMult = 3.0f; // Very heavy fall
    public float DownPressMult => downPressGravityMult;

    private Rigidbody2D _rb;
    public float DefaultGravity => defaultGravity;
    public float FallGravityMult => fallGravityMultiplier;
    public float JumpHangGravityMult => hangGravityMultiplier;
    public float JumpHangThreshold => hangThreshold;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = defaultGravity;
    }

    public bool IsGrounded() => Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);

    public void ApplyJumpImpulse()
    {
        // Direct velocity override for that "Instant" jump start
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    public void CutJumpVelocity()
    {
        if (_rb.linearVelocity.y > 0)
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * jumpCutMultiplier);
    }

    public void SetGravityScale(float scale) => _rb.gravityScale = scale;

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(boxSize.x, boxSize.y, 0));
    }
}