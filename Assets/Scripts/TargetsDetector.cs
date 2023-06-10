using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsDetector : MonoBehaviour
{
    public float radius;
    private Unit owner;
    public LinkedList<GameObject> targetList;

    private void Awake() {
        targetList = new LinkedList<GameObject>();
        owner = transform.parent.GetComponent<Unit>();
        radius = owner.unitSO.DetectRadius;
        gameObject.layer = 0;
    }

    private void Start() {
        transform.localScale = new Vector3 (radius*3, 0.001f, radius*3);
    }

    public void AddFirstToTargetList(GameObject newTarget){
        targetList.AddFirst(newTarget);
    }

    public void AddLastToTargetList(GameObject newTarget){
        targetList.AddLast(newTarget);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInChildren<Unit>() == null) return;

        if (owner.unitSO.unitType == UnitType.Enemy){
            if (other.GetComponent<EnemyUnit>() != null) return; // return if trigger is enemy
        } else {
            if (other.GetComponent<EnemyUnit>() == null) return; // return if trigger is not enemy
        }

        if (targetList.Contains(other.gameObject)) return;
        AddLastToTargetList(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (!targetList.Contains(other.gameObject)) return;
        targetList.Remove(other.gameObject);
    }
}
