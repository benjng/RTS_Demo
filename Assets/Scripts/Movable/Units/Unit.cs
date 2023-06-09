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
    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }

    public virtual void Update(){
        UpdateTarget();
        RaycastTarget();
    }

    public void UpdateTarget(){
        if (targetsDetector.targets.Count == 0) {
            currentTarget = null;
            return;
        }
        if (currentTarget == targetsDetector.targets[0]) return;
        
        currentTarget = targetsDetector.targets[0];
    }

    public void RaycastTarget(){
        if (currentTarget == null) return;
        Vector3 direction = currentTarget.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit)) {
            Debug.DrawLine(transform.position, hit.point, Color.red, 2f);
            transform.LookAt(currentTarget.transform);
        }
    }

    public virtual void OnDrawGizmos() {
        if (unitSO == null) return;
        if (unitSO.AttackRadius == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRadius);
    }
}
