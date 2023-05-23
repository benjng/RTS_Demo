using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;
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
        private PlacedObject placedObject; // the object that is placed on this grid position. Contains info of the object.

        // Constructor
        public GridObject(Grid<GridObject> grid, int x, int z){ 
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        // for setting new placeObject info into this GridObject
        public void SetPlacedObject(PlacedObject placedObject){
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, z); // inform grid that something has changed at the gridObject[x,z]
        }

        public PlacedObject GetPlacedObject(){
            return placedObject;
        }

        public void ClearPlacedObject(){
            placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild(){
            return placedObject == null;
        }

        public override string ToString()
        {   
            if (placedObject != null) {
                // Debug.Log(transform.name);
                return placedObject + "";
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

        placedObjectTypeSO = placedObjectTypeSOList[0];
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
            
            // Setting new placeObject into this gridObject, with building direction
            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);

            // Insert placedObject info into all the gridPosition occupied
            foreach (Vector2Int gridPosition in gridPositionList){
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }
            // gridObject.SetTransform(builtTransform);
        }

        if (Input.GetMouseButtonDown(2)){
            GridObject gridObject = grid.GetGridObjectByWorldPosition(GetMouseWorldPosition3D());
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null) {
                placedObject.DestroySelf();

                // **get all the grid positions that will be occupied by the building type**
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

                // Go into all gridPosition and clear the placedObject in it
                foreach (Vector2Int gridPosition in gridPositionList){
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)){
            dir = PlacedObjectTypeSO.GetNextDir(dir);
            Debug.Log(dir);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0];}
        if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1];}
        if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2];}
        if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3];}
        if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4];}
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
