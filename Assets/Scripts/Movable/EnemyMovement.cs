using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : UnitMovement
{
    void Start()
    {
        myUnit = GetComponent<Unit>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Prevent automovement when precise/atk moving
        if(myAgent.velocity.magnitude != 0) return;

        AutoMovement();
    }
}
