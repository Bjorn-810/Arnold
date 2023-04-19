using UnityEngine;
using UnityEngine.AI;

public class MediumEnemySM : StateMachine
{
    [Header("Agent PlayerTarget")]
    public Transform PlayerTarget;

    [Header("Shooting")]
    public GunHolder GunSelector;

    [Header("Variables")]
    [HideInInspector] public FieldOfView Fov;
    [HideInInspector] public NavMeshAgent Agent;
    
    public float detectionRange = 10f;

    public float wanderRadius = 10f;
    public float wanderJitter = 10f;


    [Header("States")]
    public MediumIdle IdleState;
    public MediumWandering PatrolState;
    public MediumAttacking AttackState;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;

        IdleState = new MediumIdle(this);
        PatrolState = new MediumWandering(this);
        AttackState = new MediumAttacking(this);

        Fov = GetComponent<FieldOfView>();
        Agent = GetComponent<NavMeshAgent>();
        GunSelector = GetComponent<GunHolder>();

        SetInitialState(PatrolState);
    }
}