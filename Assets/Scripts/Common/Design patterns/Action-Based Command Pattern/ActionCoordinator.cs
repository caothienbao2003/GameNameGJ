using System.Collections.Generic;
using UnityEngine;

public class ActionCoordinator : MonoBehaviour
{
    private readonly List<IAction> _activeActions = new();

    public void TryStartAction(IAction newAction)
    {
        if (!newAction.CanStart()) return;

        // 1. Check if any currently running action has HIGHER priority
        foreach (var active in _activeActions)
        {
            if (active.Priority > newAction.Priority) 
            {
                Debug.Log($"Action {newAction.GetType().Name} blocked by {active.GetType().Name}");
                return; 
            }
        }

        // 2. Stop and remove any actions with LOWER priority
        for (int i = _activeActions.Count - 1; i >= 0; i--)
        {
            if (_activeActions[i].Priority < newAction.Priority)
            {
                _activeActions[i].Stop();
                _activeActions.RemoveAt(i);
            }
        }

        // 3. Start the new action
        newAction.Start();
        _activeActions.Add(newAction);
    }

    public void TryStopAction(IAction action)
    {
        if (_activeActions.Contains(action))
        {
            action.Stop();
            _activeActions.Remove(action);
        }
    }

    private void Update()
    {
        // Tick all active actions
        for (int i = _activeActions.Count - 1; i >= 0; i--)
        {
            _activeActions[i].Update();
        }
    }

    private void FixedUpdate()
    {
        // FixedTick all active actions
        for (int i = _activeActions.Count - 1; i >= 0; i--)
        {
            _activeActions[i].FixedUpdate();
        }
    }
}