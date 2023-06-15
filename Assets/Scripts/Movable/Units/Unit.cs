using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;
    public TargetDetector targetDetector;
    public LayerMask shootableLayer;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootingInterval = 0.001f;
    [SerializeField] private float bulletSpeed = 100f;
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
        if (targetDetector.targetList.Count == 0) {
            currentTarget = null;
            return;
        }
        if (currentTarget == targetDetector.targetList.First.Value) return;
        
        currentTarget = targetDetector.targetList.First.Value;
    }

    private void DetectTarget(){
        Vector3 currentTargetPos = currentTarget.transform.position;
        // Vector3 direction = currentTargetPos - transform.position;
        
        distanceToTgt = Vector3.Distance(transform.position, currentTargetPos);

        if (distanceToTgt < unitSO.AttackRadius){
            // Debug.Log("Target in atk range");
            // Debug.DrawLine(transform.position, currentTargetPos, Color.green, 2f);
            transform.LookAt(currentTarget.transform);
            tgtInAttackRange = true;
        } else if (distanceToTgt < unitSO.DetectRadius){
            // Debug.Log("Target in detect range");
            transform.LookAt(currentTarget.transform);
            Debug.DrawLine(transform.position, currentTargetPos, Color.red, 0.5f);
            tgtInAttackRange = false;
        } else {
            tgtInAttackRange = false;
        }
    }

    private IEnumerator ShootTarget(){ 
        while (true){
            if (tgtInAttackRange) {

                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.tag = transform.tag; // add the same tag as this transform shoot (Enemy/Player)
                Vector3 randomSpread = new Vector3(
                    Random.Range(-0.2f, 0.2f), 
                    Random.Range(-0.2f, 0.2f), 
                    0
                );

                Vector3 startPos = transform.position;
                Vector3 endPos = currentTarget.transform.position + randomSpread;

                float remainingDistance = distanceToTgt;

                // Loop (Move) till it reaches Endpoint using Lerp
                while(remainingDistance > 0){
                    bullet.transform.LookAt(endPos);
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
                yield return new WaitForSeconds(shootingInterval);
            }
            yield return null;
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Damage"))) return;
        if (other.tag == transform.tag) return; // no self/team hurting
        Debug.Log("Damaged to " + transform.name);
    }
    public virtual void OnDrawGizmos() {
        if (unitSO == null) return;
        if (unitSO.AttackRadius == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRadius);
    }
}
