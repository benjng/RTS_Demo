using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public bool isUnitSelected = false;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    [SerializeField] private TargetsDetector targetsDetector;
    private Unit myUnit;
    private NavMeshAgent myAgent;
    private Camera myCam;
    private Vector3 targetPos;

    void Start()
    {
        myUnit = GetComponent<Unit>();
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Handle RMB & Precise mvmt
        if (isUnitSelected && ModeHandler.currentMode != Mode.Building) {
            CheckRMBIsEnemy();
        }

        // No other movement when moving
        if(myAgent.remainingDistance > 0) return;

        // If there is any target, move to attack position, including player assignment/automvmt
        if (targetsDetector.targetList.Count != 0){
            targetPos = GetPosByTarget(targetsDetector.targetList.First.Value);
            MoveToPos(targetPos);
        }
    }

    void CheckRMBIsEnemy(){
        if (!Input.GetMouseButtonDown(1)) return;

        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit enemyHit, Mathf.Infinity, enemyLayer)) {
            // *** Atk movement
            UpdateTarget(enemyHit);
            return;
        } 

        // *** Precise Movement
        targetPos = GetPosByRay(ray);
        if (targetPos == transform.position) return;
        MoveToPos(targetPos); // Move till reaching destination without being stopped
    }

    void UpdateTarget(RaycastHit hit){
        // Update targetList first target
        GameObject newTarget = hit.collider.gameObject;
        targetsDetector.AddFirstToTargetList(newTarget); 
    }


    // AttackMovement: Move to attackable range
    Vector3 GetPosByTarget(GameObject newTarget){
        Debug.Log("Player Attack Ordered. Move into attackable range");
        Unit newTargetUnit = newTarget.GetComponent<Unit>();

        float targetDist = Vector3.Distance(transform.position, newTarget.transform.position);
        // Debug.Log(targetDist);
        // float attackablePositionDist = targetDist - myUnit.unitSO.AttackRadius;
        // Debug.Log(attackablePositionDist);

        if (targetDist < myUnit.unitSO.AttackRadius) 
            return transform.position;

        myAgent.stoppingDistance = myUnit.unitSO.AttackRadius;
        return newTarget.transform.position;
    }

    // Move to ordered destination. Have higher priority than any other movement.
    Vector3 GetPosByRay(Ray ray){
        myAgent.stoppingDistance = 0;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            return hit.point;

        return transform.position;
    }

    void MoveToPos(Vector3 pos){
        myAgent.SetDestination(pos);
    }

}
