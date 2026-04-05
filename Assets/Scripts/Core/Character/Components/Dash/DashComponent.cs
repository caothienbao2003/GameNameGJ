using UnityEngine;
using System.Collections;

public class DashComponent : MonoBehaviour, IDashComponent
{
    [Header("Settings")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.4f;
    [SerializeField] private float postDashVelMul = 0.4f;
    private Rigidbody2D _rb;
    private bool _canDash = true;

    private void Awake()
    {
        _rb ??= GetComponent<Rigidbody2D>();
    }

    public void ExecuteDash(Vector2 direction, System.Action onComplete)
    {
        if (!_canDash) return;
        StartCoroutine(DashRoutine(direction.normalized, onComplete));
    }

    private IEnumerator DashRoutine(Vector2 dir, System.Action onComplete)
    {
        _canDash = false;
        float originalGravity = _rb.gravityScale;

        // 1. Freeze gravity for that "Celeste" weightless feel
        _rb.gravityScale = 0;
        _rb.linearVelocity = dir * dashForce;

        yield return new WaitForSeconds(dashDuration);

        // 2. Return gravity and exit
        _rb.gravityScale = originalGravity;
        onComplete?.Invoke();

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
        _rb.linearVelocity *= postDashVelMul; // Apply post-dash velocity multiplier
    }
}