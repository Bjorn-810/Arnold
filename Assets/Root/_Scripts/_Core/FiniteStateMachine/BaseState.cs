using System.Collections;

public class BaseState
{
    public string name;

    protected StateMachine stateMachine;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }
    
    public virtual void Enter() { }
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateLateUpdate() { }
    public virtual void Exit(){ }
}