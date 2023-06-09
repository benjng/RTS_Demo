using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)){
            // TODO: if raycast not touching enemyunit, 
            PlayerMovementOrder();
            return;
            // TODO: else
            // PlayerAttackOrder();
        }

        AutoMovement();
    }

    void PlayerMovementOrder(){
        if (ModeHandler.currentMode == Mode.Building) return; // No Player control when in Building mode
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)){
            myAgent.SetDestination(hit.point);
        }
    }

    void PlayerAttackOrder(){
        // TODO: Implement Player attack order
        // myAgent.SetDestination( Attack location );
    }

    void AutoMovement(){
        // TODO: Implement Unit auto movement (e.g. detected enemy auto movement)
    }
}
