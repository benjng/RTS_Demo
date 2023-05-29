using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeHandler : MonoBehaviour
{
    public static Mode currentMode = Mode.None;

    public void SwitchMode(UnitSO unitSO){
        UnitType unitType = unitSO.unitType;
        if (unitType == UnitType.Builder){
            ModeHandler.currentMode = Mode.BuilderSelected;
        } else {
            ModeHandler.currentMode = Mode.UnitSelected;
        }
    }
}
