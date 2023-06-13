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

    [SerializeField] private GameObject bullet;
    private GameObject currentTarget;
    private bool tgtInAttackRange = false;

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
        StartCoroutine(ShootTarget());
    }

    public virtual void Update(){
        GetTarget();
        if (currentTarget == null) return;

        // If there is target, look at it
        transform.LookAt(currentTarget.transform);
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
        Vector3 currentTargetPos = currentTarget.transform.position;
        Vector3 direction = currentTargetPos - transform.position;
        
        float distanceToTgt = Vector3.Distance(transform.position, currentTargetPos);

        if (distanceToTgt < unitSO.AttackRadius){
            // Debug.Log("Target in atk range");
            Debug.DrawLine(transform.position, currentTargetPos, Color.green, 2f);
            tgtInAttackRange = true;
        } else if (distanceToTgt < unitSO.DetectRadius){
            // Debug.Log("Target in detect range");
            Debug.DrawLine(transform.position, currentTargetPos, Color.red, 2f);
            tgtInAttackRange = false;
        } else {
            tgtInAttackRange = false;
        }
    }

    private IEnumerator ShootTarget(){ // TODO:
        while (true){
            if (tgtInAttackRange){
                Debug.Log("Shooting");
                Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.eulerAngles), transform);
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
    }

    public virtual void OnDrawGizmos() {
        if (unitSO == null) return;
        if (unitSO.AttackRadius == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRadius);
    }
}
