using UnityEngine;

public class PlayerStateMachineController : MonoBehaviour
{
    [SerializeField] private StateMachineComponent _stateMachineComponent;
    private StateMachineComponent stateMachineComponent
    {
        get
        {
            if (_stateMachineComponent == null)
                _stateMachineComponent = GetComponent<StateMachineComponent>();
            return _stateMachineComponent;
        }
    }

    private void Awake()
    {
        _stateMachineComponent = GetComponent<StateMachineComponent>();
    }

    private void Start()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        // stateMachineComponent.AddState(new IdleState(this));
        // stateMachineComponent.AddState(new MoveState(this));
        // stateMachineComponent.AddState(new JumpState(this));
        // stateMachineComponent.AddState(new PlayerAttackState(this));

        // stateMachineComponent.InitializeLayer(StateLayer.Movement, new PlayerIdleState(this));
        // stateMachineComponent.InitializeLayer(StateLayer.Action, new PlayerIdleState(this));
    }
}
