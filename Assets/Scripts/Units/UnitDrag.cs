using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    Camera myCam;

    //graphical
    [SerializeField] RectTransform boxVisual;

    //logical
    Rect selectionBox;

    Vector2 startPosition, endPosition;
    void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    void Update()
    {
        // clicked
        if (Input.GetMouseButtonDown(0)){
            startPosition = Input.mousePosition;
        }
        // dragging
        if (Input.GetMouseButton(0)){
            endPosition = Input.mousePosition;
            DrawVisual();
        }
        // release click
        if (Input.GetMouseButtonUp(0)){
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual(){
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        // set box graphic to the middle of start and end position
        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        // change box graphic size to fit in between start and end position
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection(){

    }

    void SelectUnits(){

    }
}
