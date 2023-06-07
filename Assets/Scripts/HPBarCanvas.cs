using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarCanvas : MonoBehaviour
{
    private Transform cam;

    private void Awake() {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
