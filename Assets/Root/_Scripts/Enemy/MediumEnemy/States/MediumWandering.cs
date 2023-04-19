using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MediumWandering : BaseState
{
    private readonly MediumEnemySM _sm;

    public MediumWandering(MediumEnemySM statemachine) : base("Patrol", statemachine)
    {
        _sm = statemachine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.Agent.stoppingDistance = 0;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        
        // If raycast detects player, change state to chase
        if (_sm.Fov.canSeePlayer)
            _sm.ChangeState(_sm.AttackState);

        if (_sm.Agent.remainingDistance <= _sm.Agent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * _sm.wanderJitter;
            randomDirection += _sm.transform.forward * _sm.wanderRadius;
            Vector3 targetPosition = _sm.transform.position + randomDirection.normalized * _sm.wanderRadius;

            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(targetPosition, out navMeshHit, _sm.wanderRadius, -1);
            _sm.Agent.SetDestination(navMeshHit.position);
        }
    }
}
