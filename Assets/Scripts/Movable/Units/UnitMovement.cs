using System.Collections.Generic;
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
        if (myAgent.stoppingDistance > 0 && myAgent.velocity.magnitude < 0.2f){
            myAgent.ResetPath();
        }

        // Prevent automovement when precise/atk moving
        if(myAgent.velocity.magnitude != 0) return;

        // Automvmt (Chase target when there is any)
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
            List<GameObject> currentUnitsSelected = UnitSelection.Instance.unitsSelected;
            int numOfSelectedUnit = currentUnitsSelected.Count;
            if (!currentUnitsSelected.Contains(gameObject)) throw new System.Exception("selected obj not found");

            int indexOfUnit = currentUnitsSelected.IndexOf(gameObject); 
            // 0 1 2 
            // 3 4 5 
            // 6 7 8
            // 9 10 11
            // 12 13 14
            
            int numOfCol = 3;

            int offsetX = indexOfUnit % numOfCol;
            int offsetZ = indexOfUnit / numOfCol;
            Debug.Log("offsetX:" + offsetX + "; offsetZ:" + offsetZ);

            // Vector3[,] posOffsetArray = new Vector3[3, Mathf.FloorToInt(numOfSelectedUnit/3)];
            // for (int x = 0; x < posOffsetArray.GetLength(0); x++){
            //     for (int z = 0; z < posOffsetArray.GetLength(1); z++){
            //         posOffsetArray[x,z] = new Vector3(x, 0, z);
            //     }
            // }
            float multiplier = 4;
            Vector3 offSetVector = new Vector3(offsetX, 0, offsetZ) * multiplier;
            targetPos += offSetVector;
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
