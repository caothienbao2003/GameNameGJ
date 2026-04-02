using UnityEngine;

public interface IAnimationService
{
    void Play(int stateHash, float crossFade = 0.1f);
    void SetFloat(int parameterHash, float value);
    void SetTrigger(int parameterHash);
}
