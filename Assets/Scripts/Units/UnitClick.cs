using UnityEngine;

// Actual Clicking mechanics (getmousebuttondown, raycast)
// send the detected gameObj to UnitSelection instance for add/remove 
public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickable;
    public LayerMask ground;
    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition); // create a ray from screen to mouse

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable)){ // cast the ray
                // hit a clickable (Shift/normal)
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
    }
}
