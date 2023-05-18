using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid grid;
    Vector3 vec;
    Ray ray;

    void Start()
    {
        grid = new Grid(4, 2, 2f, new Vector3(20, 0, 0)); 
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            grid.SetValue(GetMouseWorldPosition(), 56);
        }
        if (Input.GetMouseButtonDown(1)){
            Debug.Log(grid.GetValue(GetMouseWorldPosition()));
        }
    }

    private Vector3 GetMouseWorldPosition(){
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 1000f);
        vec = hit.point;
        // vec.y = 0f;
        return vec;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(vec, 0.2f);

        Gizmos.DrawLine(ray.origin, ray.direction*1000f);
    }
}
