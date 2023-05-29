using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit", fileName = "New Unit")]
public class UnitSO : ScriptableObject
{
    public string unitName;
    public Material material;
    public UnitType unitType;
}
