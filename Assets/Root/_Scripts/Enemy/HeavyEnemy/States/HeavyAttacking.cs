using UnityEngine;

public class HeavyAttacking : BaseState
{
    private HeavyEnemySM _sm;
    private float _time;


    public HeavyAttacking(HeavyEnemySM statemachine) : base("HeavyAttacking", statemachine)
    {
        _sm = statemachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.Agent.stoppingDistance = 2; // Set the stopping distance to keep distance while chasing
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        _time += Time.deltaTime;

        // keep turning towards player
        Vector3 targetPosition = _sm.PlayerTarget.position;
        targetPosition.y = _sm.transform.position.y; // Ignore the target's height

        Vector3 direction = targetPosition - _sm.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target
        _sm.transform.rotation = Quaternion.Slerp(_sm.transform.rotation, targetRotation, 5 * Time.deltaTime);

        if (_sm.Fov.canSeePlayer)
        {
            _sm.Agent.SetDestination(_sm.PlayerTarget.position);
            _sm.GunSelector.Shoot();
            _time = 0;
        }

        if (_time >= 0.8f)
        {
            _sm.ChangeState(_sm.PatrollingState);
        }
    }
}
