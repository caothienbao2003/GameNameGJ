using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float FaceDirection { get; private set; } = 1f;

    public void Flip(float moveDirection)
    {
        if (Mathf.Abs(moveDirection) < 0.01f) return; // Avoid flipping when idle

        FaceDirection = Mathf.Sign(moveDirection);

        bool flip = moveDirection < 0;
        {
            target.localScale = new Vector3(FaceDirection, target.localScale.y, target.localScale.z);
        }
    }

}
