using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeHandler : MonoBehaviour
{
    public static Mode currentMode = Mode.None;

    public void SwitchMode(UnitSO unitSO){
        if (unitSO == null) {
            currentMode = Mode.None;
            // Debug.Log(currentMode);  
            return;
        }

        UnitType unitType = unitSO.unitType;
        if (unitType == UnitType.Builder){
            currentMode = Mode.BuilderSelected;
        } else {
            currentMode = Mode.UnitSelected;
        }
        // Debug.Log(currentMode);  
    }
}
