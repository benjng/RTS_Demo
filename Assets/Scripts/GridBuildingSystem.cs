using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private Transform testBuilding;
    [SerializeField] private LayerMask buildableLayers;

    private Grid<GridObject> grid; // a grid building system has one grid

    #region Grid param (Width, Height, Origin, Cell size)
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float originX;
    [SerializeField] private float originZ;
    [SerializeField] private float cellSize = 10f;
    #endregion

    /* 
        GridObject Class
        a GridObject contains a grid position's data
    */
    public class GridObject{
        private Grid<GridObject> grid; // on each grid position belongs to a grid
        private int x; // each grid position has x and z
        private int z;
        private Transform transform; // the building transform that is placed on this grid position

        public GridObject(Grid<GridObject> grid, int x, int z){ // Constructor
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        // for setting new building into this GridObject
        public void SetTransform(Transform transform){
            this.transform = transform;
            grid.TriggerGridObjectChanged(x, z); // inform grid that something has changed
        }

        public void ClearTransform(){
            transform = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild(){
            return transform == null;
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + transform; // text being updated by TriggerGridObjectChanged
        }
    }
    // end of GridObject class

    private void Awake() {
        Vector3 origin = new Vector3(originX, 0, originZ);
        /* 
            Creating a new Grid instance needs:
            1. grid params
            2. gridObjects
        */
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, origin, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)){
            grid.GetXZ(GetMouseWorldPosition3D(), out int x, out int z); // find snapping location, output to x and z
            GridObject gridObject = grid.GetGridObject(x, z);

            if (!gridObject.CanBuild()) {
                Debug.Log("YOU CANNOT BUILD HERE");
                return;
            }

            // Setting new building transform into this gridObject
            Transform builtTransform = Instantiate(testBuilding, grid.GetWorldPosition(x, z), Quaternion.identity);
            gridObject.SetTransform(builtTransform);
        }
    }

    private Vector3 GetMouseWorldPosition3D(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, buildableLayers)){
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }
}
