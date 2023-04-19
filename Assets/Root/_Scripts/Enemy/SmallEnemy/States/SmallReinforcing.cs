using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.AI;

public class SmallReinforcing : BaseState
{
    private SmallEnemySM _sm;
    private GameObject _chosenFriendly;

    public SmallReinforcing(SmallEnemySM statemachine) : base("Attacking", statemachine)
    {
        _sm = statemachine;
    }

    public override void Enter()
    {
        base.Enter();
        _chosenFriendly = FindClosestObject();

        _sm.Agent.stoppingDistance = 2; // Set the stopping distance to keep distance while chasing
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_chosenFriendly != null && _sm.Agent.CalculatePath(_chosenFriendly.transform.position, _sm.Agent.path) == true)
        {
            _sm.Agent.SetDestination(_chosenFriendly.transform.position);
        }

        else
            _sm.ChangeState(_sm.AttackingState);

        // reached target enemy
        if (Vector3.Distance(_sm.transform.position, _sm.Agent.destination) <= _sm.Agent.stoppingDistance)
        {
            _sm.ChangeState(_sm.AttackingState);
            if (_chosenFriendly.GetComponent<HeavyEnemySM>() != null)
                _chosenFriendly.GetComponent<HeavyEnemySM>().Agent.SetDestination(_sm.PlayerTarget.position);

            if (_chosenFriendly.GetComponent<MediumEnemySM>() != null)
                _chosenFriendly.GetComponent<MediumEnemySM>().Agent.SetDestination(_sm.PlayerTarget.position);
        }
    }

    private GameObject FindClosestObject()
    {
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in _sm.foundObjects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(_sm.transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    return obj;
                }
            }
        }

        return null;
    }
}
