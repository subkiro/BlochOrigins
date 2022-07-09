using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionRecorder : MonoBehaviour
{
    public readonly Stack<ActionBase> _actions = new Stack<ActionBase>();
    public void Record(ActionBase action) {
        _actions.Push(action);
        action.Execute();
    }
    public void Rewind() {
        if (_actions.Count == 0) return;
        var action = _actions.Pop();
        action.Undo();
    }

    public void Reset()
    {
        _actions.Clear();
    }

    internal int GetCount()
    {
        return _actions.Count;
    }
}
