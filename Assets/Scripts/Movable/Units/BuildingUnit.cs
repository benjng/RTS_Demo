using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnit : Unit
{
    // Start is called before the first frame update
    public override void Start()
    {
        UnitSelection.Instance.unitList.Add(this.gameObject); // add this gameobject to unitList when game start
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
