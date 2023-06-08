using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    public float radian;
    private Unit unit;

    private void Awake() {
        unit = transform.parent.GetComponent<Unit>();
        gameObject.layer = 0;
    }
    
    private void Start() {
        transform.localScale = new Vector3 (radian*2, 0.001f, radian*2);
    }

    private void OnTriggerEnter(Collider other) {
        if (unit.targets.Contains(other.gameObject)) return;
        if (other.GetComponentInChildren<Unit>()){
            unit.targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!unit.targets.Contains(other.gameObject)) return;
        unit.targets.Remove(other.gameObject);
    }
}
