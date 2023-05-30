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
    }
    
    public void UpdateInfo(List<GameObject> unitsSelected){
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
        unitDescription.text += " +";
        unitDescription.text += unitsSelected.Count.ToString();
    }

    public void UpdateAction(){

    }

    public void ClearInfoAndAction(){
        unitDescription.text = "";

        if (unitIconPanel.childCount == 0) return;
        for (int i=0; i < unitIconPanel.childCount; i++) {
            Destroy(unitIconPanel.GetChild(i).gameObject);
        }
    }
}
