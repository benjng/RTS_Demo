using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;
    public TargetsDetector TargetsDetector;

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
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
