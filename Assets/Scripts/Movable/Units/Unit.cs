using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;
    public RangeDetector RangeDetector;

    public void Awake(){
        RangeDetector.radian = unitSO.DetectRange;
    }

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }

    public void DetectOpponent(){
        // Vector3.Distance()
    }

    // private void OnDrawGizmos() {
    //     if (unitSO.DetectRange == 0) return;
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, unitSO.DetectRange);
    //     if (unitSO.AttackRange == 0) return;
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, unitSO.AttackRange);
    // }
}
