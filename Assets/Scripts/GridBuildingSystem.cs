using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBuildingSystem : MonoBehaviour
{
    public event EventHandler<OnSelectedChangedEventArgs> OnSelectedChanged;
    public class OnSelectedChangedEventArgs : EventArgs {}

    public delegate void BuiltEventHandler();
    public event BuiltEventHandler BuildEventTriggered;

    public List<PlacedObjectTypeSO> placedObjectTypeSOList; 
    [SerializeField] private LayerMask buildableLayers; 
    [SerializeField] private Transform newBuildingsHolder;

    private PlacedObjectTypeSO currentPlacedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;
    private Grid<GridObject> grid; // a grid building system has one grid

    #region Grid param (Width, Height, Origin, Cell size)
    [SerializeField] private Transform origin; 
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
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

    // Singleton
    public static GridBuildingSystem Instance;

    private void Awake() {
        if (Instance != null){
            Debug.LogWarning("More than one GridBuildingSystem instance found!");
            return;
        }
        Instance = this;
        /* 
            Creating a new Grid instance needs:
            1. grid params
            2. gridObject for each grid position
        */
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, origin.position, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));

        currentPlacedObjectTypeSO = placedObjectTypeSOList[0];
    }

    private void Start(){
        AddActionBtnsListener();
    }

    private void AddActionBtnsListener(){
        for (int i = 0; i < ControlRenderer.Instance.unitActionButtons.Count; i++){
            int index = i; // Capture the index variable
            GameObject actionBtn = ControlRenderer.Instance.unitActionButtons[i];
            Button btn = actionBtn.GetComponent<Button>();
            btn.onClick.AddListener(() => OnBuildingTrigger(index));
        }
    }

    private void CheckNumKeysInput(){
        // switching placedObject 
        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            OnBuildingTrigger(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            OnBuildingTrigger(1);
        }
    }

    // both num keys/action buttons triggers
    private void OnBuildingTrigger(int index){
        // Debug.Log("hitting build action " + index.ToString());
        currentPlacedObjectTypeSO = placedObjectTypeSOList[index];
        OnSelectedChanged(this, new OnSelectedChangedEventArgs {});
        ModeHandler.currentMode = Mode.Building;
    }

    private void Update() {
        if (ModeHandler.currentMode != Mode.BuilderSelected && 
            ModeHandler.currentMode != Mode.Building) return;

        CheckNumKeysInput();

        // ===Building mode logic===
        if (ModeHandler.currentMode != Mode.Building) return;
        if (UI.isPointingUI) return; // no building when using UI

        // Enter BuildingGhost

        // When player builds (LMB) 
        if (Input.GetMouseButtonDown(0)){
            // find snapping location of the mouse click, output to x and z, fetch the current x,z GridObject from grid instance
            int snappedX, snappedZ;
            grid.GetXZ(GetMouseWorldPosition3D(), out snappedX, out snappedZ); 
            // GridObject gridObject = grid.GetGridObject(snappedX, snappedZ); 

            bool canBuild = true;
            // **get all the grid positions that will be occupied by the building type**
            List<Vector2Int> gridPositionList = currentPlacedObjectTypeSO.GetGridPositionList(new Vector2Int(snappedX, snappedZ), dir);

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

            // offset logic
            Vector2Int rotationOffset = currentPlacedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(snappedX, snappedZ) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            
            // *****Setting new placeObject into this gridObject, with building direction
            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(snappedX, snappedZ), dir, currentPlacedObjectTypeSO, newBuildingsHolder);
            BuildEventTriggered.Invoke(); // Invoke buildevent

            // Insert placedObject info into all the gridPosition occupied
            foreach (Vector2Int gridPosition in gridPositionList){
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }
        }

        // RMB/ESC
        if (Input.GetMouseButtonUp(1) || Input.GetKeyDown(KeyCode.Escape)){ // Has to be MouseButtonUp for correct order
            QuitBuilding();
        }

        // MMB
        if (Input.GetMouseButtonDown(2)){
            RemoveBuilt();
        }

        // Rotate building
        if (Input.GetKeyDown(KeyCode.R)){
            RotateBuildingGhost();
        }
    }

    private void Build(){

    }

    private void RotateBuildingGhost(){
        dir = PlacedObjectTypeSO.GetNextDir(dir);
    }

    private void QuitBuilding(){
        ModeHandler.currentMode = Mode.BuilderSelected; // Disable buildingGhost
    }

    private void RemoveBuilt(){
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

    private Vector3 GetMouseWorldPosition3D(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, buildableLayers)){
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }

    public Vector3 GetSnappedMouseWorldPosition(){
        grid.GetXZ(GetMouseWorldPosition3D(), out int x, out int z);
        Vector2Int rotationOffset = currentPlacedObjectTypeSO.GetRotationOffset(dir);
        Vector3 snappedMouseWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
        return snappedMouseWorldPosition;
    }

    public Quaternion GetPlacedObjectRotation(){
        return Quaternion.Euler(0, currentPlacedObjectTypeSO.GetRotationAngle(dir), 0);
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO(){
        return currentPlacedObjectTypeSO;
    }
}
