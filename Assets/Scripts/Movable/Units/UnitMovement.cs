using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    private Unit myUnit;
    private NavMeshAgent myAgent;
    private Camera myCam;
    [SerializeField] private TargetsDetector targetsDetector;

    void Start()
    {
        myUnit = GetComponent<Unit>();
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)){
            if (ModeHandler.currentMode == Mode.Building) return; // No Player control when in Building mode

            // Check if its locking on target or normal movement
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer)) {
                GameObject newTarget = hit.collider.gameObject;
                targetsDetector.AddFirstToTargetList(newTarget);
                AttackMovement(newTarget);
            } else {
                MoveUnitByRay(ray);
            }
            return;
        }

        AutoMovement();
    }

    // MovementOrder: Move to ordered destination. Should have higher priority than automovement.
    void MoveUnitByRay(Ray ray){
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)){
            myAgent.SetDestination(hit.point);
        }
    }

    // TODO: TargetLockOn logic
    // AttackMovement: Move to attackable range
    void AttackMovement(GameObject newTarget){
        Debug.Log("Player Attack Ordered. Move into attackable range");
        Unit newTargetUnit = newTarget.GetComponent<Unit>();
        
        newTargetUnit.isLockedOn = true;
        float targetDist = Vector3.Distance(transform.position, newTarget.transform.position);
        float attackablePositionDist = targetDist - myUnit.unitSO.AttackRadius;
        Debug.Log(targetDist);
        Debug.Log(attackablePositionDist);
        myAgent.stoppingDistance = attackablePositionDist;

        // TODO: Move agent into attack range and stop
    }

    void AutoMovement(){
        // TODO: Implement Unit auto movement (e.g. detected enemy auto movement)
    }
}
