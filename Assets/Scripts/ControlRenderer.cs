using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControlRenderer : MonoBehaviour
{
    [SerializeField] private Image unitIcon;
    [SerializeField] private TMP_Text unitName;
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

    public void UpdateInfo(UnitSO unitSO){
        unitIcon.sprite = unitSO.unitIcon;
        unitName.text = unitSO.unitName;
    }

    public void UpdateAction(){

    }

    public void ClearInfoAndAction(){
        unitIcon.sprite = null;
        unitName.text = "";
    }
}
