using System;
using UnityEngine;

public class ActionNode : Node
{
    private readonly Func<State> _action;

    public ActionNode(Func<State> action)
    {
        _action = action;
    }
    
    public override State Evaluate()
    {
        LastTickCount = Time.frameCount;
        _state = _action?.Invoke() ?? State.Failure;
        return _state;
    }
}
