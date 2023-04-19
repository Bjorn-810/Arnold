using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState _currentState;

    void Update()
    {
        if (_currentState != null)
            _currentState.StateUpdate();
    }

    void FixedUpdate()
    {
        if (_currentState != null)
            _currentState.StateFixedUpdate();
    }
    
    void LateUpdate()
    {
        if (_currentState != null)
            _currentState.StateUpdate();
    }

    /// <summary>
    /// Sets the first state of the state machine
    /// </summary>
    /// <param name="initialState"></param>
    public void SetInitialState(BaseState initialState)
    {
        _currentState = initialState;
        initialState.Enter(); 
    }

    /// <summary>
    /// Sets a new state for the state machine
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(BaseState newState)
    {
        _currentState.Exit();

        _currentState = newState;
        newState.Enter();
    }
}