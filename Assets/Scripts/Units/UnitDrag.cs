using UnityEngine;

// Renders mouse drag visuals
public class UnitDrag : MonoBehaviour
{
    Camera myCam;

    //for graphical representation
    [SerializeField] RectTransform boxVisual;

    //for logical selection calculation
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
            selectionBox = new Rect();
        }
        // dragging
        if (Input.GetMouseButton(0)){
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }
        // release click (*both unitDrag and unitClick)
        if (Input.GetMouseButtonUp(0)){
            int unitCount = SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();

            if (unitCount == 0) return;
            // Update UI
            UnitSelection.Instance.UpdateControlUI();
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
        // x calculations
        if (Input.mousePosition.x < startPosition.x){
            // dragging left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        } else {
            // dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        // y calculations
        if (Input.mousePosition.y < startPosition.y){
            // dragging down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        } else {
            // dragging up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    int SelectUnits(){
        int unitCount = 0;
        // loop thru all the units
        foreach (var unit in UnitSelection.Instance.unitList){
            unitCount++;
            // **if unit is within the bounds of the selection rect
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position))){
                // add them to selection
                UnitSelection.Instance.DragSelect(unit);
            }
        }
        return unitCount;
    }
}
