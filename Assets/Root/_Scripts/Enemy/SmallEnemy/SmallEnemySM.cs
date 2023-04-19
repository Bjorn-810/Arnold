using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallEnemySM : StateMachine {
    [Header("Agent PlayerTarget")]
    public Transform PlayerTarget;

    [Header("Shooting")]
    public GunHolder GunSelector;

    [Header("Variables")]
    public float closestFindRange;
    public List<GameObject> foundObjects;
    public LayerMask targetLayer;
    [HideInInspector] public FieldOfView Fov;
    [HideInInspector] public NavMeshAgent Agent;

    public float detectionRange = 10f;

    public float wanderRadius = 10f;
    public float wanderJitter = 10f;


    [Header("States")]
    [HideInInspector] public SmallWandering WanderingState;
    [HideInInspector] public SmallReinforcing ReinforcingState;
    [HideInInspector] public SmallAttacking AttackingState;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;

        foundObjects = new List<GameObject>();

        WanderingState = new SmallWandering(this);
        ReinforcingState = new SmallReinforcing(this);
        AttackingState = new SmallAttacking(this);

        Fov = GetComponent<FieldOfView>();
        Agent = GetComponent<NavMeshAgent>();
        GunSelector = GetComponent<GunHolder>();

        SetInitialState(WanderingState);
    }
}
