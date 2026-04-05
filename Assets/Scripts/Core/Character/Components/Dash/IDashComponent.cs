using UnityEngine;

public interface IDashComponent
{
    void ExecuteDash(Vector2 direction, System.Action onComplete);
    
}
