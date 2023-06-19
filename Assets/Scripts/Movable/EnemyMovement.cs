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

        // Make sure target is still alive
        if (targetDetector.targetList.Count == 0) return;
        if (targetDetector.targetList.First.Value == null) {
            targetDetector.targetList.RemoveFirst(); // remove empty
            return;
        }

        // Automvmt by targetList (Chase target when there is any)
        destination = GetDestinationByTarget(targetDetector.targetList.First.Value);
        MoveToPos(destination);
    }
}
