using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Cell
{
    public Vector2Int gridPosition;
    public Wall walls; //bit Encoded
    public void RemoveWall(Wall wallToRemove)
    {
        walls = (walls & ~wallToRemove);
    }

    public int GetNumWalls()
    {
        int numWalls = 0;
        if (((walls & Wall.DOWN) != 0)) { numWalls++; }
        if (((walls & Wall.UP) != 0)) { numWalls++; }
        if (((walls & Wall.LEFT) != 0)) { numWalls++; }
        if (((walls & Wall.RIGHT) != 0)) { numWalls++; }
        return numWalls;
    }

    public bool HasWall(Wall wallDirection)
    {
        return (walls & wallDirection) != 0;
    }
    
    public List<Cell> GetNeighbours(Cell[,] grid)
    {
        List<Cell> result = new List<Cell>();
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int cellX = this.gridPosition.x + x;
                int cellY = this.gridPosition.y + y;
                if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1) || Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }
                Cell canditateCell = grid[cellX, cellY];
                result.Add(canditateCell);
            }
        }
        return result;
    }
}

[System.Flags]
public enum Wall
{
    LEFT    = 1,
    UP      = 2,
    RIGHT   = 4,
    DOWN    = 8
}
