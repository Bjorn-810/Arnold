using UnityEngine;
using UnityEngine.AI;

public class HeavyEnemySM : StateMachine
{
    [Header("Agent PlayerTarget")]
    public Transform PlayerTarget;
    public Transform[] PatrolPoints;

    [Header("Shooting")]
    public GunHolder GunSelector;

    [Header("Variables")]
    [HideInInspector] public FieldOfView Fov;
    [HideInInspector] public NavMeshAgent Agent;

    [Header("States")]
    [HideInInspector] public HeavyPatrolling PatrollingState;
    [HideInInspector] public HeavyAttacking AttackingState;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;

        PatrollingState = new HeavyPatrolling(this);
        AttackingState = new HeavyAttacking(this);

        Fov = GetComponent<FieldOfView>();
        Agent = GetComponent<NavMeshAgent>();
        GunSelector = GetComponent<GunHolder>();

        SetInitialState(PatrollingState);
    }
}