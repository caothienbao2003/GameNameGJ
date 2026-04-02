using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateLayer { Movement, Action }

public class StateMachineComponent : MonoBehaviour
{
    private Dictionary<StateLayer, StateMachine> _layerMachines = new();
    private Dictionary<Type, IState> _stateLibrary = new();

    public void AddState(IState state)
    {
        _stateLibrary[state.GetType()] = state;
    }

    public void InitializeLayer(StateLayer layer, IState startingState)
    {
        var machine = new StateMachine();
        machine.Initialize(startingState);
        _layerMachines[layer] = machine;
    }

    public void TransitionLayer<T>(StateLayer layer) where T : IState
    {
        if (_stateLibrary.TryGetValue(typeof(T), out var state))
        {
            if (_layerMachines.TryGetValue(layer, out var machine))
            {
                machine.TransitionTo(state);
            }
            else
            {
                Debug.LogWarning($"Layer {layer} not initialized!");
            }
        }
    }

    private void Update()
    {
        foreach (var machine in _layerMachines.Values)
            machine.Update();
    }

    private void FixedUpdate()
    {
        foreach (var machine in _layerMachines.Values)
            machine.FixedUpdate();
    }
}