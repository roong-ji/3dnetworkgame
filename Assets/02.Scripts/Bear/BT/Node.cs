public enum State
{
    None,
    Success,
    Failure,
    Running
}

public abstract class Node
{
    public abstract State Evaluate();
    public int LastTickCount;
    
    public string Name; 
    protected State _state;
    public State CurrentState => _state;
}
