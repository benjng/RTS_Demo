using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
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
                PlayerAttackOrder();
            } else {
                PlayerMovementOrder();
            }
            return;
        }

        AutoMovement();
    }

    // MovementOrder: Move to ordered destination
    void PlayerMovementOrder(){
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)){
            myAgent.SetDestination(hit.point);
        }
    }

    // AttackOrder: Move to attackable range
    void PlayerAttackOrder(){
        // TODO: Implement Player attack order
        // myAgent.SetDestination( Attack location );
        Debug.Log("Player Attack Ordered. Move into attackable range");
    }

    void AutoMovement(){
        // TODO: Implement Unit auto movement (e.g. detected enemy auto movement)
    }
}
