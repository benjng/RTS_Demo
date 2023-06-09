using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsDetector : MonoBehaviour
{
    public float radius;
    private Unit unit;
    public List<GameObject> targets;

    private void Awake() {
        targets = new List<GameObject>();
        unit = transform.parent.GetComponent<Unit>();
        radius = unit.unitSO.DetectRadius;
        gameObject.layer = 0;
    }

    private void Start() {
        transform.localScale = new Vector3 (radius*3, 0.001f, radius*3);
    }

    private void OnTriggerEnter(Collider other) {
        if (targets.Contains(other.gameObject)) return;
        if (other.GetComponentInChildren<Unit>()){
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!targets.Contains(other.gameObject)) return;
        targets.Remove(other.gameObject);
    }
}
