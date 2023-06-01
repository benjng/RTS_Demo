using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControlRenderer : MonoBehaviour
{
    // [SerializeField] private Image unitIcon;
    [SerializeField] private Transform unitIconPanel;
    [SerializeField] private TMP_Text unitDescription;
    [SerializeField] private Transform unitActionPanel;
    [SerializeField] private TMP_Text unitActionDescription;
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
        // ActionPanel.SetActive(false);
        ClearInfoAndAction();
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
