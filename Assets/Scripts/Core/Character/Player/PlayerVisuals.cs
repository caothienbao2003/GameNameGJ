using UnityEngine;
using DG.Tweening;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private JumpPhysicsComponent jumpMotor;

    [Header("Dynamic Velocity Stretch")]
    [SerializeField] private float stretchFactor = 0.03f;
    [SerializeField] private float maxStretch = 1.35f;
    [SerializeField] private float minStretch = 0.65f;
    [SerializeField] private float stretchSmoothTime = 0.1f; // Higher = more "floaty" at apex

    [Header("Impact Squash")]
    [SerializeField] private Vector3 landSquash = new Vector3(1.4f, 0.6f, 1f);
    [SerializeField] private float landDuration = 0.12f; // Increased for less "snap"

    private Vector3 _originalScale;
    private bool _isLanding;
    private Vector3 _currentVelocityScale;

    private void Awake()
    {
        _originalScale = spriteTransform.localScale;
        rb = rb ?? GetComponentInParent<Rigidbody2D>();
        jumpMotor = jumpMotor ?? GetComponentInParent<JumpPhysicsComponent>();
    }

    private void OnEnable()
    {
        jumpMotor.OnLandEvent += PlayLandEffect;
        jumpMotor.OnJumpEvent += PlayJumpEffect;
    }

    private void OnDisable()
    {
        jumpMotor.OnLandEvent -= PlayLandEffect;
        jumpMotor.OnJumpEvent -= PlayJumpEffect;
    }

    private void Update()
    {
        // While landing, we stop the velocity-stretch to let the tween finish
        if (_isLanding) return;

        HandleDynamicStretch();
    }

    private void HandleDynamicStretch()
    {
        float yVel = rb.linearVelocity.y;
        
        // Calculate target stretch
        float stretch = 1 + (Mathf.Abs(yVel) * stretchFactor);
        stretch = Mathf.Clamp(stretch, minStretch, maxStretch);
        float inverseStretch = 1 / stretch;

        Vector3 targetScale = new Vector3(
            _originalScale.x * inverseStretch,
            _originalScale.y * stretch,
            _originalScale.z
        );

        // --- THE FIX FOR SNAPPINESS ---
        // Use SmoothDamp or Lerp so the shape "hangs" longer at the apex (zero velocity)
        spriteTransform.localScale = Vector3.SmoothDamp(
            spriteTransform.localScale, 
            targetScale, 
            ref _currentVelocityScale, 
            stretchSmoothTime
        );
    }

    private void PlayLandEffect()
    {
        _isLanding = true;
        spriteTransform.DOKill();
        
        Sequence s = DOTween.Sequence();
        
        // Squash Down
        s.Append(spriteTransform.DOScale(landSquash, landDuration).SetEase(Ease.OutQuad));
        // Bounce back to neutral
        s.Append(spriteTransform.DOScale(_originalScale, landDuration).SetEase(Ease.OutBack));
        
        s.OnComplete(() => _isLanding = false);
    }

    private void PlayJumpEffect()
    {
        // If we are landing, let the land effect finish a tiny bit 
        // OR kill it to show the jump power. 
        // For "Immediate Jump" feel, we kill it but add a strong Pop.
        _isLanding = false; 
        spriteTransform.DOKill();
        
        // Takeoff "Pop": Thin and Tall
        spriteTransform.DOScale(new Vector3(0.6f, 1.4f, 1f), 0.1f)
            .SetEase(Ease.OutQuad);
    }
}