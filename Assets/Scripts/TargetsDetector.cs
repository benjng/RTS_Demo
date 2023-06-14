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
        if (targetList.Contains(newTarget))
            targetList.Remove(newTarget);
        targetList.AddFirst(newTarget);
    }

    public void AddLastToTargetList(GameObject newTarget){
        targetList.AddLast(newTarget);
    }

    public void ClearTargetList(){
        targetList.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        Unit unit = other.GetComponentInChildren<Unit>();
        EnemyUnit enemyUnit = other.GetComponent<EnemyUnit>();

        if (unit == null)
            return;

        // Only the opposite team units will be added to targetList
        if (owner.unitSO.unitType == UnitType.Enemy) {
            if (enemyUnit != null) return;
        } else {
            if (enemyUnit == null) return;
        }

        if (targetList.Contains(other.gameObject))
            return;

        AddLastToTargetList(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (!targetList.Contains(other.gameObject)) return;
        targetList.Remove(other.gameObject);
    }
}
