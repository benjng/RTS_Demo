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

    [SerializeField] private GameObject bulletPrefab;
    private GameObject currentTarget;
    private bool tgtInAttackRange = false;
    private float distanceToTgt;

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
        StartCoroutine(ShootTarget());
    }

    public virtual void Update(){
        GetTarget();
        if (currentTarget == null) return;
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
        
        distanceToTgt = Vector3.Distance(transform.position, currentTargetPos);

        if (distanceToTgt < unitSO.AttackRadius){
            // Debug.Log("Target in atk range");
            // Debug.DrawLine(transform.position, currentTargetPos, Color.green, 2f);
            tgtInAttackRange = true;
        } else if (distanceToTgt < unitSO.DetectRadius){
            // Debug.Log("Target in detect range");
            transform.LookAt(currentTarget.transform);
            Debug.DrawLine(transform.position, currentTargetPos, Color.red, 2f);
            tgtInAttackRange = false;
        } else {
            tgtInAttackRange = false;
        }
    }

    private IEnumerator ShootTarget(){ // TODO:
        while (true){
            if (tgtInAttackRange) {

                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, transform);
                Vector3 startPos = transform.position;
                Vector3 endPos = currentTarget.transform.position;
                float bulletSpeed = 10f;
                float remainingDistance = distanceToTgt;

                // Loop (Move) till it reaches Endpoint using Lerp
                while(remainingDistance > 0){
                    bullet.transform.position = Vector3.Lerp(
                        startPos,
                        endPos,
                        Mathf.Clamp01(1 - (remainingDistance / distanceToTgt))
                    );
                    remainingDistance -= bulletSpeed * Time.deltaTime;
                    yield return null;
                }

                bullet.transform.position = endPos; // Make sure it reaches EndPoint
                Destroy(bullet, 0.1f);
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
