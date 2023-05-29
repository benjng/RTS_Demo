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

    public void UpdateInfo(Image newImage, TMP_Text newUnitName){
        unitIcon = newImage;
        unitName.text = newUnitName.text;
    }

    public void UpdateAction(){

    }

    public void ClearInfoAndAction(){
        
    }
}
