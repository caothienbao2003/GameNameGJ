using UnityEngine;

public class MoveHorizontalComponent : MonoBehaviour, IMoveToDirection
{
    [Header("Celeste-Style Settings")]
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float acceleration = 60f;  // How fast we reach max speed
    [SerializeField] private float deceleration = 80f;  // How fast we stop
    [SerializeField] private float frictionMultiplier = 2.5f; // Extra "grip" when turning

    private Rigidbody2D _rb;
    private Rigidbody2D rb => _rb ??= GetComponent<Rigidbody2D>();

    public void MoveToDirection(Vector3 direction)
    {
        // 1. Calculate the velocity we WANT to have
        float targetSpeed = direction.x * maxSpeed;

        // 2. Determine which friction/acceleration rate to use
        float lerpAmount;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            // We are actively pushing a direction
            lerpAmount = acceleration;

            // --- TURN AROUND LOGIC ---
            // If we are moving Right (vel > 0) but pressing Left (target < 0) or vice versa
            bool isTurning = (targetSpeed > 0 && rb.linearVelocity.x < -0.01f) ||
                             (targetSpeed < 0 && rb.linearVelocity.x > 0.01f);

            if (isTurning)
            {
                // Apply massive friction to flip the character's momentum instantly
                lerpAmount *= frictionMultiplier;
            }
        }
        else
        {
            // We let go of the keys, use deceleration
            lerpAmount = deceleration;
        }

        // 3. Apply the movement over time (FixedDeltaTime for physics consistency)
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, lerpAmount * Time.fixedDeltaTime);

        // 4. Update the Rigidbody (Keeping Y velocity for jumping!)
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    public void Stop()
    {
        // Instead of a hard 0, we "MoveTo" zero to allow the slide to finish
        MoveToDirection(Vector3.zero);
    }
}