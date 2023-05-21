using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building", fileName = "New building")]
public class PlacedObjectTypeSO : ScriptableObject
{
    public enum Dir {Up, Down, Left, Right}
    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir){
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        for (int x = 0; x < width; x++){
            for (int z = 0; z < height; z++){
                gridPositionList.Add(offset + new Vector2Int(x, z));
            }
        }
        return gridPositionList;
    }
}
