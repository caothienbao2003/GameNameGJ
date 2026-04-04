using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpPhysicsComponent : MonoBehaviour, IJumpComponent
{
    public event Action OnJumpEvent;
    public event Action OnLandEvent;
    [Header("Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f);

    [Header("Base Physics")]
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float defaultGravity = 4.5f;
    [SerializeField] private float jumpCutMultiplier = 0.3f;

    [Header("Feel Multipliers")]
    [SerializeField] private float fallGravityMultiplier = 2.0f;
    [SerializeField] private float hangGravityMultiplier = 0.4f;
    [SerializeField] private float hangThreshold = 2.5f;
    [SerializeField] private float downPressGravityMult = 3.5f;

    [Header("Grace Periods")]
    [SerializeField] private float coyoteTimeThreshold = 0.15f;
    
    private Rigidbody2D _rb;
    private float _coyoteTimeCounter;
    private bool _wasGroundedLastFrame;
    private float _lastFrameYVelocity;

    public float DefaultGravity => defaultGravity;
    public float FallGravityMult => fallGravityMultiplier;
    public float JumpHangGravityMult => hangGravityMultiplier;
    public float JumpHangThreshold => hangThreshold;
    public float DownPressMult => downPressGravityMult;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = defaultGravity;
    }

    private void FixedUpdate()
    {
        // Remember velocity before physics resolution clears it on impact
        _lastFrameYVelocity = _rb.linearVelocity.y;
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();

        // Landing Detection Logic
        if (isGrounded && !_wasGroundedLastFrame)
        {
            if (_lastFrameYVelocity < -1.5f) // Threshold to avoid micro-squash on slopes
            {
                OnLandEvent?.Invoke();
            }
        }

        _wasGroundedLastFrame = isGrounded;

        if (isGrounded) _coyoteTimeCounter = coyoteTimeThreshold;
        else _coyoteTimeCounter -= Time.deltaTime;
    }

    public bool IsGrounded() => Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);
    public bool IsCoyoteTimeActive() => _coyoteTimeCounter > 0;
    public void ConsumeCoyoteTime() => _coyoteTimeCounter = 0;

    public void ApplyJumpImpulse()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        OnJumpEvent?.Invoke();
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
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }
}