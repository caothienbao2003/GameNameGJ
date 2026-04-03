public interface IAction
{
    int Priority { get; set;}
    bool CanStart();
    void Start();
    void Update();
    void FixedUpdate();
    void Stop();
}
