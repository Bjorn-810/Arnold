using System.Collections;
using UnityEngine;

public class HeavyPatrolling : BaseState
{
    private readonly HeavyEnemySM _sm;

    private Transform[] _patrolpoints;
    private int _patrolPointIndex = 0;
    private bool _movingForward = true;

    public HeavyPatrolling(HeavyEnemySM statemachine) : base("HeavyPatrolling", statemachine)
    {
        _sm = statemachine;
    }

    public override void Enter()
    {
        base.Enter();

        if (_sm.PatrolPoints.Length == 0)
            return;

        _patrolpoints = _sm.PatrolPoints;
        _sm.Agent.stoppingDistance = 0;
        SetNextPatrolPoint();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        // If raycast detects player, change state to chase
        if (_sm.Fov.canSeePlayer)
            _sm.ChangeState(_sm.AttackingState);

        HandlePatrolling();
    }

    private void HandlePatrolling()
    {
        if (_sm.PatrolPoints.Length == 0)
            return;

        if (_sm.Agent.remainingDistance <= _sm.Agent.stoppingDistance) // If reached patrolpoint        
            SetNextPatrolPoint();

        _sm.Agent.SetDestination(_patrolpoints[_patrolPointIndex].position);
    }

    private void SetNextPatrolPoint()
    {
        if (_patrolPointIndex == _patrolpoints.Length - 1)
            _movingForward = false;

        else if (_patrolPointIndex == 0)
            _movingForward = true;

        // if moving forward, add 1 to the current patrol point
        if (_movingForward)
            _patrolPointIndex++;

        else
            _patrolPointIndex--;
    }

    #region Tip silvano
    private void Test()
    {
        stateMachine.StartCoroutine(Test2());
    }

    private IEnumerator Test2()
    {
        yield return new WaitForSeconds(1);
    }
    #endregion
}
