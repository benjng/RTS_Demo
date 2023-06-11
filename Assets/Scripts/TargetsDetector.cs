using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsDetector : MonoBehaviour
{
    public float detectRadius;
    private Unit owner;
    public LinkedList<GameObject> targetList;
    public List<GameObject> debugTgtList;

    private void Awake() {
        targetList = new LinkedList<GameObject>(); 
        debugTgtList = new List<GameObject>();
        owner = transform.parent.GetComponent<Unit>();
        detectRadius = owner.unitSO.DetectRadius;
        gameObject.layer = 0;
    }

    private void Start() {
        transform.localScale = new Vector3 (detectRadius*3, 0.001f, detectRadius*3);
    }

    private void Update() {
        debugTgtList = targetList.ToList();
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
        if (other.gameObject == targetList.First.Value) return; // always lockon first target
        targetList.Remove(other.gameObject);
    }
}
