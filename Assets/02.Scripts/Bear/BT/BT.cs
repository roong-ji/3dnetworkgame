using UnityEngine;

public abstract class BT : MonoBehaviour
{
    private Node _root;
    
#if UNITY_EDITOR
    public Node Root => _root;
#endif
    
    protected void Start()
    {
        _root = SetupTree();
    }
    
    protected void Update()
    {
        _root?.Evaluate();
    }

    protected abstract Node SetupTree();
}
