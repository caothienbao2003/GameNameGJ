public interface IAnimationService
{
    // Control with Priority
    void Play(int stateHash, int priority = 0, float crossFade = 0.1f);
    void PlayForce(int stateHash, int priority = 0, float crossFade = 0.05f);
    
    // Manual Lock Management
    void ReleasePriority();
    void SetPriority(int priority);

    // Parameters
    void SetFloat(int paramHash, float value);
    void SetBool(int paramHash, bool value);
    void Trigger(int paramHash);

    // Data Info
    bool IsInState(int stateHash, int layer = 0);
    float GetCurrentProgress(int layer = 0);
}