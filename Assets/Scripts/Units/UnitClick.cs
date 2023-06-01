using UnityEngine;

// Actual Clicking mechanics (getmousebuttondown, raycast)
// send the detected gameObj to UnitSelection instance for add/remove 
public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickable;
    public LayerMask ground;
    Ray ray;
    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){ //LMB
            RaycastHit hit;
            ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse

            // hit a clickable (Shift/normal)
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable)){ // cast the ray
                if (Input.GetKey(KeyCode.LeftShift)){
                    UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                } else {
                    UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                }
            } else {
                // didn't hit a clickable
                if (!Input.GetKey(KeyCode.LeftShift))
                    UnitSelection.Instance.DeselectAll();
            }
        }

        // if (Input.GetMouseButtonDown(1)){ //RMB
        //     RaycastHit hit;
        //     Ray ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse

        //     if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground)){ // cast the ray
                
        //     }
        // }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction*70);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ray.origin, ray.direction*70);
        Gizmos.DrawSphere(ray.direction*70, 2f);
    }
}
