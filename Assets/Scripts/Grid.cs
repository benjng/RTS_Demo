using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    private int width;
    private int height;
    private float yOffset = 1.5f;
    private float cellSize;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Vector3 originPosition;

    public Grid(int width, int height, float cellSize, Vector3 originPosition) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++){
            for (int z = 0; z < gridArray.GetLength(1); z++){
                debugTextArray[x, z] = CreateWorldText(gridArray[x, z].ToString(), null, GetWorldPosition(x, z) + (new Vector3(cellSize, 0, cellSize) * .5f), 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
    }

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 5, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000){
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder){
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.rotation = Quaternion.Euler(90f, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    private Vector3 GetWorldPosition(int x, int z){
        return new Vector3(x, yOffset/cellSize, z) * cellSize + originPosition;
    }

    // Get grid position x and z from mouse world position
    private void GetXZ(Vector3 worldPosition, out int x, out int z){
        x = Mathf.FloorToInt((worldPosition-originPosition).x/cellSize);
        z = Mathf.FloorToInt((worldPosition-originPosition).z/cellSize);
    }

    // Set value of the visible grid text
    public void SetValue(int x, int z, int value){
        if (x >= 0 && z >= 0 && x < width && z < height){
            gridArray[x, z] = value;
            debugTextArray[x, z].text = gridArray[x, z].ToString();
        } 
    }

    public void SetValue(Vector3 worldPosition, int value){
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    public int GetValue(int x, int z){
        if (x >= 0 && z >= 0 && x < width && z < height){
            return gridArray[x, z];
        } else {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition){
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}
