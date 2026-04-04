using UnityEngine;

public class AnimatorComponent : MonoBehaviour, IAnimationService
{
    [SerializeField] private Animator _animator;

    private void Awake() => _animator ??= GetComponentInChildren<Animator>();

    // These are now wrappers to feed the state machine
    public void SetFloat(int paramHash, float value) => _animator.SetFloat(paramHash, value);
    public void SetBool(int paramHash, bool value) => _animator.SetBool(paramHash, value);
    public void Trigger(int paramHash) => _animator.SetTrigger(paramHash);

    // If you still need to play an animation "instantly" (like a hit reaction)
    public void Play(int stateHash, int priority = 0, float crossFade = 0.1f) 
    {
        _animator.CrossFadeInFixedTime(stateHash, crossFade);
    }

    public void PlayForce(int stateHash, int priority = 0, float crossFade = 0.05f)
    {
        _animator.PlayInFixedTime(stateHash, -1, 0f);
    }

    // Standard Helpers
    public bool IsInState(int stateHash, int layer = 0) 
        => _animator.GetCurrentAnimatorStateInfo(layer).shortNameHash == stateHash;

    public float GetCurrentProgress(int layer = 0) 
    {
        if (_animator.IsInTransition(layer)) return 0f;
        return _animator.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1f;
    }

    // Priority methods are kept empty or removed to maintain IAnimationService interface
    public void ReleasePriority() { }
    public void SetPriority(int priority) { }
}