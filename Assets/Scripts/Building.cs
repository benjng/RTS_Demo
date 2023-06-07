using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingTypeSO;
    private Vector2Int origin;
    private BuildingTypeSO.Dir dir;
    
    public static Building Create(Vector3 worldPosition, Vector2Int origin, BuildingTypeSO.Dir dir, BuildingTypeSO buildingTypeSO, Transform parent){
        Transform buildingTransform = Instantiate(buildingTypeSO.prefab, worldPosition, Quaternion.Euler(0, buildingTypeSO.GetRotationAngle(dir), 0), parent);

        Building building = buildingTransform.GetComponent<Building>();

        building.buildingTypeSO = buildingTypeSO;
        building.origin = origin;
        building.dir = dir;

        return building;
    }

    public List<Vector2Int> GetGridPositionList(){
        return buildingTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}
