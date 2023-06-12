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
        // Handle RMB & atk mvmt && Precise mvmt
        if (isUnitSelected && ModeHandler.currentMode != Mode.Building) {
            CheckRMBIsEnemy();
        }

        // Prevent jittering
        if (myAgent.stoppingDistance > 0 && myAgent.velocity.magnitude < 0.1){
            myAgent.ResetPath();
        }

        // Prevent automovement when precise/atk moving
        Debug.Log(myAgent.velocity.magnitude);
        if(myAgent.velocity.magnitude != 0) return;

        if (targetsDetector.targetList.Count == 0) return; // && current action finished
        // Automvmt
        targetPos = GetPosByTarget(targetsDetector.targetList.First.Value);
        MoveToPos(targetPos);
    }

    void CheckRMBIsEnemy(){
        if (!Input.GetMouseButtonDown(1)) return;

        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit enemyHit, Mathf.Infinity, enemyLayer)) {
            // *** Atk movement
            UpdateTarget(enemyHit);
            targetPos = GetPosByTarget(targetsDetector.targetList.First.Value);
        } else {
            // *** Precise Movement
            targetPos = GetPosByRay(ray);
        }

        MoveToPos(targetPos); // Move till reaching destination without being stopped
    }

    void UpdateTarget(RaycastHit hit){
        // Update targetList first target
        GameObject newTarget = hit.collider.gameObject;
        targetsDetector.AddFirstToTargetList(newTarget); 
    }


    // AttackMovement: Move to attackable range
    Vector3 GetPosByTarget(GameObject newTarget){
        float targetDist = Vector3.Distance(transform.position, newTarget.transform.position);

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
        if (pos == transform.position) return;
        myAgent.SetDestination(pos);
    }

}
