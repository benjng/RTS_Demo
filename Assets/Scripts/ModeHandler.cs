using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeHandler : MonoBehaviour
{
    public static Mode currentMode = Mode.None;
    [SerializeField] private TMP_Text debugText;

    void Update(){
        debugText.text = currentMode.ToString();
    }

    public void SwitchModeByUnitSO(UnitSO unitSO){
        if (unitSO == null) {
            currentMode = Mode.None;
            return;
        }

        UnitType unitType = unitSO.unitType;
        if (unitType == UnitType.Builder){
            currentMode = Mode.BuilderSelected;
        } else {
            currentMode = Mode.SoliderSelected;
        }
    }
}
