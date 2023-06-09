using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;
    public TargetsDetector targetsDetector;

    private GameObject currentTarget;
    RaycastHit hit;

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }

    public virtual void LateUpdate(){
        if (targetsDetector.targets.Count == 0) return;
        Debug.Log(targetsDetector.targets[0]);
        currentTarget = targetsDetector.targets[0];
        Vector3 direction = currentTarget.transform.position - transform.position;
        Physics.Raycast(transform.position, direction.normalized, out hit);
        Debug.DrawLine(transform.position, hit.point, Color.red, 2f);
    }


    public virtual void OnDrawGizmos() {
        // if (unitSO.DetectRadius == 0) return;
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, unitSO.DetectRadius);
        if (unitSO.AttackRadius == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRadius);
    }
}
