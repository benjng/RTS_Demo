using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHP : MonoBehaviour
{
    private Transform cam;
    private UnitSO unitSO;
    private Slider slider;

    private void Awake(){
        cam = Camera.main.transform;
        unitSO = transform.parent.GetComponent<Unit>().unitSO;
        slider = transform.GetComponentInChildren<Slider>();
    }
    
    private void Start(){
        slider.maxValue = unitSO.MaxHP;
        slider.value = slider.maxValue;
        slider.onValueChanged.AddListener(OnHPZero);
    }

    private void LateUpdate(){
        transform.LookAt(transform.position + cam.forward);
    }

    public void DeductHP(int amount){
        if (amount <= 0) return;
        slider.value -= amount;
    }

    public void AddHP(int amount){
        if (amount >= slider.maxValue) return;
        slider.value -= amount;
    }

    private void OnHPZero(float value){
        slider.onValueChanged.RemoveListener(OnHPZero);
        if (value <= 0) {
            // TODO: destroy unit
            // Destroy(gameObject, 1);
        }
    }
}
