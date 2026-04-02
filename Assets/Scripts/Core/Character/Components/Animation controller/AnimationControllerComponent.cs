using UnityEngine;

public class AnimationControllerComponent : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        // Automatically finds the animator on this object or its children
        _animator = GetComponentInChildren<Animator>();
    }

    public void Play(int stateHash, float crossFade = 0.1f)
    {
        if (_animator != null) _animator.CrossFade(stateHash, crossFade);
    }

    public void SetFloat(int parameterHash, float value)
    {
        if (_animator != null) _animator.SetFloat(parameterHash, value);
    }

    public void SetTrigger(int parameterHash)
    {
        if (_animator != null) _animator.SetTrigger(parameterHash);
    }
}
