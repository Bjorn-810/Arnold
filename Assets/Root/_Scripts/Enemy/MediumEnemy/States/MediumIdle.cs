using UnityEngine;

[System.Serializable]
public class MediumIdle : BaseState
{
    private MediumEnemySM _sm;

    public MediumIdle(MediumEnemySM statemachine) : base("Idle", statemachine) {
        _sm = statemachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }
}
