using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building", fileName = "New building")]
public class PlacedObjectTypeSO : ScriptableObject
{
    public int GetRotationAngle(Dir dir){
        return (int)dir * 90;
    }

    public Vector2Int GetRotationOffset(Dir dir){
        switch (dir){
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, width);
            case Dir.Up: return new Vector2Int(width, height);
            case Dir.Right: return new Vector2Int(height, 0);
        }
    }

    public static Dir GetNextDir(Dir dir){
        if ((int)dir >= 3) return 0;
        return dir + 1;
    }
    public enum Dir { Down, Left, Up, Right, }

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    // Check the position that will be occuipied and return the list
    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir){
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir) {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++){
                    for (int z = 0; z < height; z++){
                        gridPositionList.Add(offset + new Vector2Int(x, z));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++){
                    for (int z = 0; z < width; z++){
                        gridPositionList.Add(offset + new Vector2Int(x, z));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}
