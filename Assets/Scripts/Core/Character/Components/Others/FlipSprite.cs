using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Flip(float moveDirection)
    {
        if (Mathf.Abs(moveDirection) < 0.01f) return; // Avoid flipping when idle
        bool flip = moveDirection < 0;
        {
            spriteRenderer.flipX = flip;
        }
    }

}
