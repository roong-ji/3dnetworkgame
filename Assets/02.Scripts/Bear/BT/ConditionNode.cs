using System;
using UnityEngine;

public class ConditionNode : Node
{
    private readonly Func<bool> _condition;

    public ConditionNode(Func<bool> condition)
    {
        _condition = condition;
    }
    
    public override State Evaluate()
    {
        LastTickCount = Time.frameCount;
        
        var result = _condition();
        _state =  result ? State.Success : State.Failure;

        return _state;
    }
}
