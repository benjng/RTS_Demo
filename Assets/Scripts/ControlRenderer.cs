using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControlRenderer : MonoBehaviour
{
    public List<GameObject> unitActionButtons = new List<GameObject>();
    [SerializeField] private Transform unitIconPanel;
    [SerializeField] private TMP_Text unitDescription;
    [SerializeField] private Transform unitActionPanel;
    [SerializeField] private TMP_Text unitActionDescription;
    [SerializeField] private GameObject buildingBtnPrefab;


    private static ControlRenderer instance;
    public static ControlRenderer Instance { get { return instance; }}
    void Awake(){
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    } 

    private void Start() {
        ClearInfoAndAction();
        CreateActionButtons(); // render action butons when game start
    }
    
    public void UpdateInfoAndAction(List<GameObject> unitsSelected){
        ClearInfoAndAction();
        ShowInfo(unitsSelected);
        ShowAction(unitsSelected);

        if (unitsSelected.Count <= 1) return;
        // if more than 1 unit selected
        unitDescription.text += " +";
        unitDescription.text += (unitsSelected.Count - 1).ToString();
    }

    private void ShowInfo(List<GameObject> unitsSelected){
        for (int i=0; i < unitsSelected.Count; i++){
            UnitSO unitSO = unitsSelected[i].GetComponent<Unit>().unitSO;
            if (i == 0) {
                unitDescription.text = unitSO.unitName;
            }
            GameObject unitIcon = new GameObject("unitIcon");
            unitIcon.transform.SetParent(unitIconPanel);
            Image unitIconImg = unitIcon.AddComponent<Image>();
            unitIconImg.sprite = unitSO.unitIcon;
        }
    }

    private void CreateActionButtons(){
        //TODO: add button functionality
        foreach (PlacedObjectTypeSO placedObjectType in GridBuildingSystem.Instance.placedObjectTypeSOList){
            //TODO: change button sprite to building sprite
            GameObject buildingBtn = Instantiate(buildingBtnPrefab, unitActionPanel);
            unitActionButtons.Add(buildingBtn);
            GameObject buildingText = buildingBtn.transform.GetChild(0).gameObject;
            TMP_Text text = buildingText.GetComponent<TMP_Text>();
            text.text = placedObjectType.nameString;
        }
    }

    private void ShowAction(List<GameObject> unitsSelected){
        foreach (GameObject unit in unitsSelected){
            UnitSO unitSO = unit.GetComponent<Unit>().unitSO;
            if(unitSO.unitType == UnitType.Builder){
                // If unitsSelected contains any builder unit, bring up building list
                unitActionPanel.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void ClearInfoAndAction(){
        unitActionPanel.gameObject.SetActive(false);
        unitDescription.text = "";

        if (unitIconPanel.childCount == 0) return;
        for (int i=0; i < unitIconPanel.childCount; i++) {
            Destroy(unitIconPanel.GetChild(i).gameObject);
        }
    }
}
