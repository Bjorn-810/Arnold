using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform[] _PatrolPoints;
    private GameObject _playerTarget;

    [SerializeField] private float _DetectionRange = 5f;

    private Transform _currentTarget;

    private int _currentPatrolpoint;

    private bool _isGoingForward;



    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerTarget = FindObjectOfType<CharacterControllerScript>().gameObject;
    }

    void Update()
    {
        // if player is in sight
        if (PlayerWithinRadius())
            _currentTarget = _playerTarget.transform;

        // patrolling
        else if (ReachedDestination())
        {

        }

        //sets the destiantion
        _agent.SetDestination(_currentTarget.position);
    }

    private bool ReachedDestination()
    {
        return _agent.remainingDistance <= _agent.stoppingDistance;
    }


    private Transform GetClosestPatrolPoint()
    {
        float closestDistance = float.MaxValue; // saves the closest distance

        for (int i = 0; i < _PatrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, _PatrolPoints[i].position);

            if (distance < closestDistance) // makes it so the transform with the least amount of distance is returned
                return _PatrolPoints[i];
        }

        return null;
    }

    private bool PlayerWithinRadius()
    {
        return Vector3.Distance(_playerTarget.transform.position, transform.position) < _DetectionRange;
    }
}
