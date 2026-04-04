using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStateMachineController : SerializedMonoBehaviour
{
    [SerializeField] private StateMachineComponent _stateMachineComponent;
    private StateMachineComponent stateMachineComponent => _stateMachineComponent ??= GetComponent<StateMachineComponent>();
    [SerializeField] private IPlayerInput _inputEvents;
    private IPlayerInput inputEvents => _inputEvents ??= GetComponent<IPlayerInput>();
    [SerializeField] private IMoveToDirection _moveToDirection;
    private IMoveToDirection moveToDirection => _moveToDirection ??= GetComponent<IMoveToDirection>();

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
        var idleState = new IdleState(inputEvents);
        var moveState = new MoveState(moveToDirection);

        stateMachineComponent.AddState(idleState);
        stateMachineComponent.AddState(moveState);

        // stateMachineComponent.AddState(new MoveState(this));
        // stateMachineComponent.AddState(new JumpState(this));
        // stateMachineComponent.AddState(new PlayerAttackState(this));

        // stateMachineComponent.InitializeLayer(StateLayer.Movement, new PlayerIdleState(this));
        // stateMachineComponent.InitializeLayer(StateLayer.Action, new PlayerIdleState(this));
    }
}
