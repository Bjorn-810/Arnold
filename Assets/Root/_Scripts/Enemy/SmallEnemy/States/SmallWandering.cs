using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class SmallWandering : BaseState
{
    private readonly SmallEnemySM _sm;

    public SmallWandering(SmallEnemySM statemachine) : base("Patrol", statemachine)
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

        FindAllies();

        // If raycast detects player, change state to chase
        if (_sm.Fov.canSeePlayer)
        {
            _sm.ChangeState(_sm.ReinforcingState);
        }

        if (_sm.Agent.remainingDistance <= 1f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * _sm.wanderJitter;
            randomDirection += _sm.transform.forward * _sm.wanderRadius;
            Vector3 targetPosition = _sm.transform.position + randomDirection.normalized * _sm.wanderRadius;

            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(targetPosition, out navMeshHit, _sm.wanderRadius, -1);
            _sm.Agent.SetDestination(navMeshHit.position);
        }
    }

    private void FindAllies()
    {
        Collider[] colliders = Physics.OverlapSphere(_sm.transform.position, _sm.closestFindRange);

        // Remove destroyed or out-of-range objects from the list
        _sm.foundObjects = _sm.foundObjects
            .Where(obj => obj != null && Vector3.Distance(obj.transform.position, _sm.transform.position) <= _sm.closestFindRange)
            .ToList();

        // Find new allies within the range and add them to the list
        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            // Skip if the collider belongs to the same game object as the script
            if (obj == _sm.gameObject)
                continue;

            // If the object is on the target layer and is not in the list already, add it.
            if ((_sm.targetLayer.value & (1 << obj.layer)) != 0 && !_sm.foundObjects.Contains(obj))
            {
                _sm.foundObjects.Add(obj);
            }
        }
    }
}
