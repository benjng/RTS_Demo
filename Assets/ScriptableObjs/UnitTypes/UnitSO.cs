using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit", fileName = "New Unit")]
public class UnitSO : ScriptableObject
{
    public string unitName;
    public Material material;
    public Sprite unitIcon;
    public UnitType unitType;
}
