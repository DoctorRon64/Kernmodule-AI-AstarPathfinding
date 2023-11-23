using System.Collections.Generic;
using UnityEngine;

public partial class Astar
{
	public List<Vector2Int> FindPathToTarget(Vector2Int _startPos, Vector2Int _endPos, Cell[,] _grid)
	{
		Node[,] nodeGrid = CreateNodeGrid(_grid, _startPos, _endPos);
		Node startNode = FindStartingNode(_startPos, _grid, nodeGrid);
		Cell startCell = FindStartingCell(_startPos, _grid);

		int rows = nodeGrid.GetLength(0);
		int cols = nodeGrid.GetLength(1);
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Debug.Log(nodeGrid[i, j].FScore);
			}
		}

		//List<Cell> Neighbours = startCell.GetNeighbours(_grid);

		return null;
	}
	public Node[,] CreateNodeGrid(Cell[,] _grid,Vector2Int _startPos, Vector2Int _endPos)
	{
		int rows = _grid.GetLength(0);
		int cols = _grid.GetLength(1);

		Node[,] nodeGrid = new Node[rows, cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Vector2Int nodePos = _grid[i, j].gridPosition;
				nodeGrid[i, j] = new Node(nodePos, null, DefineGScore(nodePos, _startPos), DefineHScore(nodePos, _endPos));
			}
		}
		return nodeGrid;
	}
	public int DefineGScore(Vector2Int _NeighbourNodePos, Vector2Int _currentNodePos)
	{
		int dx = Mathf.Abs(_NeighbourNodePos.x - _currentNodePos.x);
		int dy = Mathf.Abs(_NeighbourNodePos.y - _currentNodePos.y);
		int stepCost = 10;
		int diagonalCost = 14;
		int distance = 0;

		while (dx > 0 || dy > 0)
		{
			if (dx > 0 && dy > 0)
			{
				distance += diagonalCost;
				dx--;
				dy--;
			}
			else
			{
				distance += stepCost;
				if (dx > 0) dx--;
				if (dy > 0) dy--;
			}
		}
		return distance * 10;
	}
	public int DefineHScore(Vector2Int _NeighbourNodePos, Vector2Int _endPos)
	{
		int dx = Mathf.Abs(_NeighbourNodePos.x - _endPos.x);
		int dy = Mathf.Abs(_NeighbourNodePos.y - _endPos.y);

		int stepCost = 10;
		int diagonalCost = 14;
		int distance = 0;

		while (dx > 0 || dy > 0)
		{
			if (dx > 0 && dy > 0)
			{
				distance += diagonalCost;
				dx--;
				dy--;
			}
			else
			{
				distance += stepCost;
				if (dx > 0) dx--;
				if (dy > 0) dy--;
			}
		}
		return distance * 10;
	}
	public Node FindStartingNode(Vector2Int _startPos, Cell[,] _grid, Node[,] _nodeGrid)
	{
		int rows = _grid.GetLength(0);
		int cols = _grid.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Cell currentCell = _grid[i, j];
				if (currentCell.gridPosition == _startPos)
				{
					Node foundNode = _nodeGrid[i, j];
					return foundNode;
				}
			}
		}
		return null;
	}
	public Cell FindStartingCell(Vector2Int _startPos, Cell[,] _grid)
	{
		int rows = _grid.GetLength(0);
		int cols = _grid.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				Cell currentCell = _grid[i, j];
				if (currentCell.gridPosition == _startPos)
				{
					return currentCell;
				}
			}
		}
		return null;
	}

	public class Node
	{
		public Vector2Int position; //Position on the grid
		public Node parent; //Parent Node of this node

		//GScore + HScore
		public float FScore
		{ 
			get { return GScore + HScore; }
		}
		public float GScore; //Current Travelled Distance
		public float HScore; //Distance estimated based on Heuristic

		public Node() { }

		public Node(Vector2Int position, Node parent, int GScore, int HScore)
		{
			this.position = position;
			this.parent = parent;
			this.GScore = GScore;
			this.HScore = HScore;
		}
	}
}
