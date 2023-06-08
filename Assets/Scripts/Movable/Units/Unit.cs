using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;
    public TargetsDetector TargetsDetector;

    private GameObject currentTarget;

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }

    public void Update(){
        if (TargetsDetector.targets.Count == 0) return;
        currentTarget = TargetsDetector.targets[0]; // TODO: Fix no target assigned
        RaycastHit hit;
        Physics.Raycast(transform.position, currentTarget.transform.position, out hit);
        Debug.DrawLine(transform.position, hit.point, Color.red);
    }

    private void OnDrawGizmos() {
        // if (unitSO.DetectRadius == 0) return;
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, unitSO.DetectRadius);
        if (unitSO.AttackRadius == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRadius);
    }
}
