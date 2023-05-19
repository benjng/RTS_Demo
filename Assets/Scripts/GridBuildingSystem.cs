using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    private Grid<GridObject> grid;
    
    private void Awake() {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        // grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, () => new GridObject());
        // grid = new Grid<bool>(gridWidth, gridHeight, cellSize, Vector3.zero, () => 1);
    }

    public class GridObject{
        private Grid<GridObject> grid;
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
