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
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd){
        DeselectAll();
        unitsSelected.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true); // selection indicator
        unitToAdd.GetComponent<UnitMovement>().enabled = true;

        UnitSO unitSO = unitToAdd.GetComponent<UnitSO>();
        modeHandler.SwitchMode(unitSO);
    }

    public void ShiftClickSelect(GameObject unitToAdd){
        if(!unitsSelected.Contains(unitToAdd)){
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        } else {
            unitToAdd.GetComponent<UnitMovement>().enabled = false;
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitsSelected.Remove(unitToAdd);
        }
    }

    public void DragSelect(GameObject unitToAdd){
        if (!unitsSelected.Contains(unitToAdd)){
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void DeselectAll(){
        foreach (var unit in unitsSelected){
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
        unitsSelected.Clear();
    }

    public void Deselect(GameObject unitToDeselect){

    }
}
