using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;
    public TargetsDetector targetsDetector;
    public LayerMask shootableLayer;

    private GameObject currentTarget;
    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }

    public virtual void Update(){
        GetTarget();
        if (currentTarget == null)
            return;
        // if 
        DetectTarget();
    }

    public void GetTarget(){
        if (targetsDetector.targetList.Count == 0) {
            currentTarget = null;
            return;
        }
        if (currentTarget == targetsDetector.targetList.First.Value) return;
        
        currentTarget = targetsDetector.targetList.First.Value;
    }

    private void DetectTarget(){
        Vector3 direction = currentTarget.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, Mathf.Infinity, shootableLayer)) {
            Debug.DrawLine(transform.position, hit.point, Color.red, 2f);
            transform.LookAt(currentTarget.transform);
            ShootTarget(hit);
        }
    }

    private void ShootTarget(RaycastHit hit){
        float distanceToTgt = Vector3.Distance(transform.position, currentTarget.transform.position);
        Debug.Log(distanceToTgt);
        if (distanceToTgt < unitSO.AttackRadius){
            Debug.DrawLine(transform.position, hit.point, Color.green, 2f);
        }
    }

    public virtual void OnDrawGizmos() {
        if (unitSO == null) return;
        if (unitSO.AttackRadius == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRadius);
    }
}
