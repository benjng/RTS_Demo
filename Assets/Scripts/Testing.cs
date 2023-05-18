using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid grid;

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
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.y = 0f;
        return vec;
    }
}
