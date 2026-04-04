using System.Collections.Generic;
using UnityEngine;

public class ActionCoordinatorComponent : MonoBehaviour
{
    private readonly List<IAction> _activeActions = new();

    public bool TryStartAction(IAction newAction)
    {
        if (newAction == null) return false;
        if (!newAction.CanStart()) return false;
        if (_activeActions.Contains(newAction)) return false;

        // 1. Check if any currently running action has HIGHER priority
        foreach (var active in _activeActions)
        {
            if (active.Priority > newAction.Priority)
            {
                return false;
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

        return true;
    }

    public bool TryStopAction(IAction action)
    {
        if (_activeActions.Contains(action))
        {
            action.Stop();
            _activeActions.Remove(action);
            return true;
        }
        return false;
    }

    private void Update()
    {
        for (int i = _activeActions.Count - 1; i >= 0; i--)
        {
            IAction action = _activeActions[i];

            // 1. Check if the action finished itself (e.g., reached the ground)
            if (action.IsFinished())
            {
                action.Stop();
                _activeActions.RemoveAt(i);
                continue;
            }

            action.Update();
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