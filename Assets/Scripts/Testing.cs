using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid grid;
    Vector3 vec;

    void Start()
    {
        grid = new Grid(4, 2, 10f); 
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            grid.SetValue(GetMouseWorldPosition(), 56);
        }
    }

    private Vector3 GetMouseWorldPosition(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        vec = hit.point;
        vec.y = 0f;
        return vec;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(vec, 2f);
    }
}
