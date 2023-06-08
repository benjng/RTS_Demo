using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    public float radian;
    private void Awake() {
        gameObject.layer = 0;
    }
    private void Start() {
        transform.localScale = new Vector3 (radian*2, 0.001f, radian*2);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInChildren<Unit>()){
            print(other);
        }
    }
}
