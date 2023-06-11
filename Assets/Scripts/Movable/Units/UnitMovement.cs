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
        if (isUnitSelected) 
            OnMouseRightClicked();

        if (targetsDetector.targetList.Count != 0){
            targetPos = GetAttackMovementPos(targetsDetector.targetList.First.Value);
            MoveToPos(targetPos);
        }

    }

    void OnMouseRightClicked(){
        if (Input.GetMouseButtonDown(1)){
            if (ModeHandler.currentMode == Mode.Building) return; // No Player control when in Building mode

            // Check if its locking on target or normal movement
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer)) {
                // *** Atk movement
                GameObject newTarget = hit.collider.gameObject;
                targetsDetector.AddFirstToTargetList(newTarget);
            } else {
                // *** Precise Movement
                targetsDetector.ClearTargetList();
                targetPos = GetPreciseMovementPos(ray);
                if (targetPos == transform.position) return;
                MoveToPos(targetPos);
            }
        }
    }

    // AttackMovement: Move to attackable range
    Vector3 GetAttackMovementPos(GameObject newTarget){
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

    // GetPreciseMovementPos: Move to ordered destination. Should have higher priority than automovement.
    Vector3 GetPreciseMovementPos(Ray ray){
        myAgent.stoppingDistance = 0;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            return hit.point;

        return transform.position;
    }

    void MoveToPos(Vector3 pos){
        myAgent.SetDestination(pos);
    }

}
