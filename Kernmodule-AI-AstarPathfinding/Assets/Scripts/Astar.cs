using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public partial class Astar
{
	public List<Vector2Int> FindPathToTarget(Vector2Int _startPos, Vector2Int _endPos, Cell[,] _grid)
	{
		//calculating some variables
		List<Vector2Int> EndPath = new List<Vector2Int>();
		Node[,] nodeGrid = CreateNodeGrid(_grid, _startPos, _endPos);
		Node startNode = FindNodeInNodegrid(_startPos, _grid, nodeGrid);
		Node endNode = FindNodeInNodegrid(_endPos, _grid, nodeGrid);

		//Start Checking
		List<Node> openList = new List<Node>();
		List<Node> closeList = new List<Node>();
		openList.Add(startNode);

		//Loop Checking
		while (openList.Count > 0)
		{
			Node currentNode = GetNodeWithLowestFScore(openList);
			openList.Remove(currentNode);
			closeList.Add(currentNode);

			//WE FOUND THE END PATH
			if (currentNode.position == _endPos)
			{
				//Run a method that will look trough all parents 
				List<Vector2Int> path = returnTheTruePath(endNode, startNode);
				return path;
			}

			//get the neighbour nodes
			Cell currentCell = FindCellInGrid(currentNode.position, _grid);
			List<Cell> Neighbours = currentCell.GetNeighbours(_grid);
			List<Node> neighbourNodes = new List<Node>();
			foreach (Cell neighbor in Neighbours)
			{
				neighbourNodes.Add(FindNodeInNodegrid(neighbor.gridPosition, _grid, nodeGrid));
			}

			foreach (Node neighbouringnode in neighbourNodes)
			{
				// if in close list skip to next node
				if (closeList.Contains(neighbouringnode))
				{
					continue;
				}

				//check if there is a shorter neigbhour node
				if (neighbouringnode.FScore <= currentNode.FScore || !openList.Contains(neighbouringnode))

				{
					neighbouringnode.parent = currentNode;
					if (!openList.Contains(neighbouringnode))
					{
						openList.Add(neighbouringnode);
					}
				}
			}
		}

		return EndPath;
	}

	//Generating The Node Grid//////////////////////////////////
	private Node[,] CreateNodeGrid(Cell[,] _grid, Vector2Int _startPos, Vector2Int _endPos)
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
	private int DefineGScore(Vector2Int _NeighbourNodePos, Vector2Int _currentNodePos)
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
	private int DefineHScore(Vector2Int _NeighbourNodePos, Vector2Int _endPos)
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

	//Getting diffrent Values/////////////////////////////////////
	private Node FindNodeInNodegrid(Vector2Int _startPos, Cell[,] _grid, Node[,] _nodeGrid)
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
	private Cell FindCellInGrid(Vector2Int _startPos, Cell[,] _grid)
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
	private Node GetNodeWithLowestFScore(List<Node> _listNodes)
	{
		if (_listNodes.Count == 0)
		{
			return null;
		}

		Node lowestFScoreNode = _listNodes[0];
		foreach (Node node in _listNodes)
		{
			if (node.FScore < lowestFScoreNode.FScore)
			{
				lowestFScoreNode = node;
			}
		}
		return lowestFScoreNode;
	}
	private List<Vector2Int> returnTheTruePath(Node _endNode, Node _startNode)
	{
		List<Vector2Int> path = new List<Vector2Int>();
		Node currentNode = _endNode;

		while (currentNode != _startNode)
		{
			path.Add(currentNode.position);
			currentNode = currentNode.parent;
			if (currentNode == null) { break; }
		}

		path.Reverse();
		return path;
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
