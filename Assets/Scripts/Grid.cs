using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject> {

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged; // <OGVCE> specifies the type of the event arguments that will be passed when the event is triggered
    public class OnGridValueChangedEventArgs : EventArgs { // hold additional infromation related to the event
        public int x;
        public int z;
    }

    private int width;
    private int height;
    private float yOffset = 0.2f;
    private float cellSize;
    private TGridObject[,] gridArray; // Generic type array, takes care whatever type that comes in
    private TextMesh[,] debugTextArray;
    private Vector3 originPosition;

    // Constructor
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) { // Func<>: a method signature
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        // Init gridObject to gridArray
        for (int x = 0; x < gridArray.GetLength(0); x++){
            for (int z = 0; z < gridArray.GetLength(1); z++){
                // createGridObject(): create a GridObject with its Grid data (width,etc) and x,z
                gridArray[x, z] = createGridObject(this, x, z); 
            }
        }

        // For debug text on screen (grid lines, coordinates, gridObject names)
        bool showDebug = false;
        if (showDebug){
            debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++){
                for (int z = 0; z < gridArray.GetLength(1); z++){
                    debugTextArray[x, z] = CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + (new Vector3(cellSize, 0, cellSize) * .5f), 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);

            // When Value is changed, run the following
            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                // Debug.Log(gridArray[eventArgs.x, eventArgs.z]?.ToString());
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }
    // end of Grid constructor

    // Create visible coordination text (debug)
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 5, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000){
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder){
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        // transform.rotation = Quaternion.Euler(90f, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public Vector3 GetWorldPosition(int x, int z){
        return new Vector3(x, yOffset/cellSize, z) * cellSize + originPosition;
    }

    // Get grid position x and z from mouse world position (*SNAPPING logic)
    public void GetXZ(Vector3 worldPosition, out int x, out int z){
        x = Mathf.FloorToInt((worldPosition-originPosition).x/cellSize);
        z = Mathf.FloorToInt((worldPosition-originPosition).z/cellSize);
    }

    public void TriggerGridObjectChanged(int x, int z){
        if (OnGridValueChanged != null) {
            OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z }); // pass new x and z to arg
        }
    }

    public void SetGridObject(int x, int z, TGridObject value){
        if (x >= 0 && z >= 0 && x < width && z < height){
            gridArray[x, z] = value;
            OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
    }

    public TGridObject GetGridObjectByWorldPosition(Vector3 worldPosition){
        GetXZ(worldPosition, out int x, out int z);
        return GetGridObject(x, z);
    }

    public TGridObject GetGridObject(int x, int z){
        if (x >= 0 && z >= 0 && x < width && z < height)
            return gridArray[x, z];
        else 
            return default(TGridObject);
    }

    public float GetCellSize(){
        return this.cellSize;
    }
}
