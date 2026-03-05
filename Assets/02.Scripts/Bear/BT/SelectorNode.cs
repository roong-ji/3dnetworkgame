using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    private readonly List<Node> _children;

    public SelectorNode(List<Node> children)
    {
        _children = children;
    }

    public override State Evaluate()
    {
        LastTickCount = Time.frameCount;
        
        foreach (var child in _children)
        {
            var state = child.Evaluate();
            _state = state;
            if (state is not State.Failure) return state;
        }

        return State.Failure;
    }
}
