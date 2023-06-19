using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    // TODO: Enemy unit logics here
    // TODO: Take care also enemy movement? (separate from UnitMovement)

    // public override void Start(){
    //     base.Start();
    // }
    public override void Update(){
        base.Update();
        if (targetDetector.targetList.Count == 0) return;
        if (targetDetector.targetList.First.Value == null) {
            targetDetector.targetList.RemoveFirst();
            return;
        }
    }

    // public override void OnDrawGizmos(){}
}
