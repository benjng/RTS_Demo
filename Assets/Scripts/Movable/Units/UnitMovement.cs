using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    [SerializeField] private TargetsDetector targetsDetector;

    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)){
            if (ModeHandler.currentMode == Mode.Building) return; // No Player control when in Building mode

            // if raycast not touching enemyunit
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer)) {
                GameObject newTarget = hit.collider.gameObject;
                targetsDetector.AddFirstToTargetList(newTarget);
                AttackMovement(newTarget);
            } else {
                DestinatedMovement();
            }
            return;
        }

        AutoMovement();
    }

    // MovementOrder: Move to ordered destination. Should have higher priority than automovement.
    void DestinatedMovement(){
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)){
            myAgent.SetDestination(hit.point);
        }
    }

    // AttackOrder: Move to attackable range
    void AttackMovement(GameObject newTarget){
        // TODO: Implement Player attack order
        // TODO: Move agent into attack range and stop
        Debug.Log("Player Attack Ordered. Move into attackable range");
    }

    void AutoMovement(){
        // TODO: Implement Unit auto movement (e.g. detected enemy auto movement)
    }
}
