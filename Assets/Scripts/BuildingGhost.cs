using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private Transform visual;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private void Start() {
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;      
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e){
        RefreshVisual();
    }

    private void LateUpdate(){
        if (ModeHandler.currentMode != Mode.Building) {
            if (visual != null) DestroyVisual();
            return;
        }

        Vector3 targetPosition = GridBuildingSystem.Instance.GetSnappedMouseWorldPosition();
        targetPosition.y = 1f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }

    // Visual: for building preview
    private void RefreshVisual(){
        if (visual != null){
            Destroy(visual.gameObject);
            visual = null;
        }

        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

        if (placedObjectTypeSO != null){
            visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            // SetLayerRecursive(visual.gameObject, 11);
        }
    }

    private void DestroyVisual(){
        Destroy(visual.gameObject);
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer){
        Debug.Log("SetLayerRecurisve");
    }
}
