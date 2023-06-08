using System.Collections.Generic;
using UnityEngine;

// Takes care the list/unlist logics
public class UnitSelection : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    [SerializeField] private ModeHandler modeHandler;

    private static UnitSelection instance;
    public static UnitSelection Instance { get { return instance; }}

    void Awake(){
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    } 

    public void ClickSelect(GameObject unitToAdd){
        DeselectAll();
        ActivateUnit(unitToAdd);
        SwitchMode(unitToAdd);
    }

    public void ShiftClickSelect(GameObject unitToAdd){
        if(!unitsSelected.Contains(unitToAdd)){
            ActivateUnit(unitToAdd);
        } else {
            // deselect unit
            if (unitToAdd.TryGetComponent<UnitMovement>(out UnitMovement unitMovement)){
                unitMovement.enabled = false;
            }
            unitToAdd.GetComponent<Unit>().HPBarCanvas.SetActive(false);
            unitsSelected.Remove(unitToAdd);
        }
        SwitchMode(unitToAdd);
    }

    public void DragSelect(GameObject unitToAdd){
        if (unitsSelected.Contains(unitToAdd)) return;

        ActivateUnit(unitToAdd);
        SwitchMode(unitToAdd);
    }

    public void DeselectAll(){
        foreach (var unit in unitsSelected){
            if (unit.TryGetComponent<UnitMovement>(out UnitMovement unitMovement)){
                unitMovement.enabled = false;
            }
            unit.GetComponent<Unit>().HPBarCanvas.SetActive(false); // visual indicator
        }
        unitsSelected.Clear();
        modeHandler.SwitchModeByUnitSO(null);
        ClearControlUI();
    }

    public void UpdateControlUI(){
        // Debug.Log("UpdateControlUI");
        ControlRenderer.Instance.UpdateInfoAndAction(unitsSelected);
    }

    public void ClearControlUI(){
        // Debug.Log("ClearControlUI");
        // Clear out UI info and action
        ControlRenderer.Instance.ClearInfoAndAction();
    }

    // 1. add selected unit to list, 2. activate indicator, 3. enable unit movement
    private void ActivateUnit(GameObject unitToAdd){
        unitsSelected.Add(unitToAdd);
        unitToAdd.GetComponent<Unit>().HPBarCanvas.SetActive(true); // visual indicator
        if (unitToAdd.TryGetComponent<UnitMovement>(out UnitMovement unitMovement)){
            unitMovement.enabled = true;
        }
    }

    private void SwitchMode(GameObject unit){
        UnitSO unitSO = unit.GetComponent<Unit>().unitSO;
        modeHandler.SwitchModeByUnitSO(unitSO);
    }
}
