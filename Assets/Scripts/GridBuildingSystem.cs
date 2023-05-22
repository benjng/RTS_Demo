using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSO;
    [SerializeField] private LayerMask buildableLayers; 
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;
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

        // Constructor
        public GridObject(Grid<GridObject> grid, int x, int z){ 
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
            if (transform != null) {
                // Debug.Log(transform.name);
                return transform + "";
            }

            return x + ", " + z;
        }
    }
    // end of GridObject class

    private void Awake() {
        Vector3 origin = new Vector3(originX, 0, originZ);
        /* 
            Creating a new Grid instance needs:
            1. grid params
            2. gridObject for each grid position
        */
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, origin, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
    }

    private void Update() {
        // When player builds (RMB)
        if (Input.GetMouseButtonDown(1)){
            // find snapping location, output to x and z, fetch the current x,z GridObject from grid instance
            grid.GetXZ(GetMouseWorldPosition3D(), out int x, out int z); 
            GridObject gridObject = grid.GetGridObject(x, z); 

            bool canBuild = true;
            // **get all the grid positions that will be occupied by the building type**
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, z), dir);

            // if ANY gridPosition cannot be built, no build is allowed
            foreach (Vector2Int gridPosition in gridPositionList){
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()){
                    canBuild = false;
                    break;
                }
            }

            // Check if any existing building in all occupied area
            if (!canBuild) {
                Debug.Log("YOU CANNOT BUILD HERE");
                return;
            }

            // ===Can build logic===

            // offset logic
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            
            // Setting new building transform into this gridObject, with building direction
            Transform builtTransform = Instantiate(
                placedObjectTypeSO.prefab, 
                placedObjectWorldPosition, 
                Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0)
            );

            // Insert transform info into all the gridPosition occupied
            foreach (Vector2Int gridPosition in gridPositionList){
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(builtTransform);
            }
            gridObject.SetTransform(builtTransform);
        }

        if (Input.GetKeyDown(KeyCode.R)){
            dir = PlacedObjectTypeSO.GetNextDir(dir);
            Debug.Log(dir);
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
