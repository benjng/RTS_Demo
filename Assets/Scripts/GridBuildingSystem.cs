using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    private Grid<GridObject> grid;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float originX;
    [SerializeField] private float originZ;
    [SerializeField] private float cellSize = 10f;
    
    private void Awake() {
        // int gridWidth = 10;
        // int gridHeight = 10;
        // float cellSize = 10f;
        Vector3 origin = new Vector3(originX, 0, originZ);
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, origin, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
        // grid = new Grid<bool>(gridWidth, gridHeight, cellSize, Vector3.zero, () => 1);
    }

    public class GridObject{
        private Grid<GridObject> grid; // on each grid position contains a grid object
        private int x;
        private int z;

        public GridObject(Grid<GridObject> grid, int x, int z){
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return x + ", " + z;
        }
    }
}
