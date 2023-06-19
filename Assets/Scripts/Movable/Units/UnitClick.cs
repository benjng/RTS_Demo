using UnityEngine;

// send the detected gameObj to UnitSelection instance for add/remove 
public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickSelectableUnit;
    // public LayerMask ui;
    // public LayerMask ground;
    Ray ray;
    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        if (UI.isPointingUI) return; // no unit detection when cursor is inside UI

        if (Input.GetKeyDown(KeyCode.Escape)){ // ESC
            UnitSelection.Instance.DeselectAll();
        }

        if (Input.GetMouseButtonDown(0)){ //LMB
            RaycastHit hit;
            ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse

            // hit a clickSelectableUnit (Shift/normal)
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickSelectableUnit)){ // cast the ray
                if (Input.GetKey(KeyCode.LeftShift)){
                    UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                } else {
                    UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                }
            } else {
                // didn't hit a clickSelectableUnit (clicking nothing)
                if (!Input.GetKey(KeyCode.LeftShift))
                    UnitSelection.Instance.DeselectAll();
            }
        }

        // TODO: TargetSelection here?
        // if (Input.GetMouseButtonDown(1)){ //RMB
        //     if (ModeHandler.currentMode != Mode.SoldierSelected) return;
        //     RaycastHit hit;
        //     Ray ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse

        //     if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickSelectableUnit)){ // cast the ray
        //         Debug.Log(hit.collider.gameObject.transform.parent);
        //     }
        // }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(ray.origin, ray.direction*70);
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(ray.origin, ray.direction*70);
        // Gizmos.DrawSphere(ray.direction*70, 2f);
    }
}
