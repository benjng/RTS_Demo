using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Unit
{
    public override void Start()
    {
        UnitSelection.Instance.unitList.Add(this.gameObject); // add this gameobject to unitList when game start
    }

    // TODO: Building unit logics

    void OnDestroy() {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
