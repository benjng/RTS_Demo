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
    public bool isLockedOn = false;

    private GameObject currentTarget;
    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }

    public virtual void Update(){
        UpdateTarget();
        if (currentTarget == null) {
            // Debug.Log("No target found.");
            return;
        }
        ShootTarget();
    }

    public void UpdateTarget(){
        if (targetsDetector.targetList.Count == 0) {
            currentTarget = null;
            return;
        }
        if (currentTarget == targetsDetector.targetList.First.Value) return;
        
        currentTarget = targetsDetector.targetList.First.Value;
    }

    public void ShootTarget(){
        // TODO: Implement shooting logic
        // Debug.Log("Current target exists. Shooting target.");
        Vector3 direction = currentTarget.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, Mathf.Infinity, shootableLayer)) {
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
