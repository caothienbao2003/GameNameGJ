public interface IAction
{
    int Priority { get; set;}
    bool CanStart();
    bool IsFinished();
    void Start();
    void Update();
    void FixedUpdate();
    void Stop();
}
