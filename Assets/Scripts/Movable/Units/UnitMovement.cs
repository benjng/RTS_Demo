using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public bool isUnitSelected = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] protected TargetDetector targetDetector;
    [SerializeField] private float unitFormationGap = 2;
    protected Unit myUnit;
    protected NavMeshAgent myAgent;
    protected Camera myCam;
    protected Vector3 destination;
    private Ray ray;

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
        if (myAgent.stoppingDistance > 0 && myAgent.velocity.magnitude < 0.2f){
            myAgent.ResetPath();
        }
        
        // Prevent automovement when precise moving
        if(myAgent.velocity.magnitude != 0) {
            Debug.Log("moving");
            return;
        }

        // TODO: Improve chasing steering flexibility
        Movement();
    }

    void ManualMovement(){
        if (!Input.GetMouseButtonDown(1)) return;
        ray = myCam.ScreenPointToRay(Input.mousePosition);

        // *** Player ordered Atk movement
        if(Physics.Raycast(ray, out RaycastHit enemyHit, Mathf.Infinity, enemyLayer, QueryTriggerInteraction.Collide)) {
            UpdateTarget(enemyHit);
            return;
        } 
        // *** Player ordered Precise movement
        myAgent.stoppingDistance = 0;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)) {
            destination = hit.point;
        } else {
            destination = transform.position;
        }

        // On Multiple units selected form formation by offset
        List<GameObject> currentUnitsSelected = UnitSelection.Instance.unitsSelected;
        int numOfSelectedUnit = currentUnitsSelected.Count;
        if (numOfSelectedUnit > 1) {
            destination += GetOffSetVector(numOfSelectedUnit, currentUnitsSelected);
        }

        // Move till reaching destination without being stopped
        if (destination != null)
            MoveToPos(destination); 
    }

    protected void Movement(){
        // Make sure target is still alive
        if (targetDetector.targetList.Count == 0) return;
        if (targetDetector.targetList.First.Value == null) {
            targetDetector.targetList.RemoveFirst();
            return;
        }

        // Movement include player targeted movements, automovements
        destination = GetDestinationByTarget(targetDetector.targetList.First.Value);
        MoveToPos(destination);
    }

    void UpdateTarget(RaycastHit hit){
        // Update targetList first target
        GameObject newTarget = hit.collider.gameObject;
        targetDetector.AddFirstToTargetList(newTarget); 
    }


    // AttackMovement: Move to attackable range
    protected Vector3 GetDestinationByTarget(GameObject newTarget){
        float targetDist = Vector3.Distance(transform.position, newTarget.transform.position);

        if (targetDist < myUnit.unitSO.AttackRadius) 
            return transform.position;

        myAgent.stoppingDistance = myUnit.unitSO.AttackRadius;
        return newTarget.transform.position;
    }

    // Move to ordered destination. Have higher priority than any other movement.
    // Vector3 GetDestinationByRay(Ray ray){
    //     myAgent.stoppingDistance = 0;
    //     if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
    //         return hit.point;

    //     return transform.position;
    // }

    Vector3 GetOffSetVector(int numOfSelectedUnit, List<GameObject> currentUnitsSelected){
        if (!currentUnitsSelected.Contains(gameObject)) throw new System.Exception("selected obj not found in list");
        int indexOfUnit = currentUnitsSelected.IndexOf(gameObject); 
        
        // units formation
        int numOfCol;
        if (numOfSelectedUnit % 2 == 0){
            numOfCol = 2;
        } else {
            numOfCol = 3;
        }

        int offsetX = indexOfUnit % numOfCol;
        int offsetZ = indexOfUnit / numOfCol;

        Vector3 offSetVector = new Vector3(offsetX, 0, offsetZ) * unitFormationGap;
        return offSetVector;
    }

    protected void MoveToPos(Vector3 pos){
        if (pos == transform.position) return;
        myAgent.SetDestination(pos);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray.origin, ray.direction*70);
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(ray.origin, ray.direction*70);
        // Gizmos.DrawSphere(ray.direction*70, 2f);
    }
}
