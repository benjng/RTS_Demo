using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid<bool> grid;
    Vector3 vec;
    Ray ray;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float originX;
    [SerializeField] private float originZ;
    [SerializeField] private float cellSize = 4f;

    void Start()
    {
        // grid = new Grid<bool>(gridWidth, gridHeight, cellSize, new Vector3(originX, 0, originZ)); 
    }
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)){
        //     grid.SetValue(GetMouseWorldPosition(), true);
        // }
        // if (Input.GetMouseButtonDown(1)){
        //     Debug.Log(grid.GetValue(GetMouseWorldPosition()));
        // }
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
