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
        myAgent = GetComponent<NavMeshAgent>();
        myCam = Camera.main;
    }

    void Update()
    {
        // Handle RMB & atk mvmt && Precise mvmt
        if (isUnitSelected && ModeHandler.currentMode != Mode.Building) {
            ManualMovement();
        }

        // Prevent jittering
        if (myAgent.stoppingDistance > 0 && myAgent.velocity.magnitude < 0.1){
            myAgent.ResetPath();
        }

        // Prevent automovement when precise/atk moving
        if(myAgent.velocity.magnitude != 0) return;

        // Automvmt
        if (targetsDetector.targetList.Count == 0) return;
        targetPos = GetPosByTarget(targetsDetector.targetList.First.Value);
        MoveToPos(targetPos);
    }

    void ManualMovement(){
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

        // Move till reaching destination without being stopped
        MoveToPos(targetPos); 
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
