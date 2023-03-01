using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Pathfinding : Singleton<Pathfinding>
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    [SerializeField] private Transform gridDebugObject;
    [SerializeField] private LayerMask obstacleLayerMask;
    
    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;
    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(int width, int height, float cellSize)
    {
        _height = height;
        _width = width;
        _cellSize = cellSize;
        _gridSystem = new GridSystem<PathNode>(width,height,cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //_gridSystem.CreateDebugObjects(gridDebugObject);

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var gridPos = new GridPosition(x, z);
                Vector3 worldPos = LevelGrid.Instance.GetWorldPosition(gridPos);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPos + Vector3.down * raycastOffsetDistance,
                        Vector3.up,
                        raycastOffsetDistance * 2,
                        obstacleLayerMask))
                {
                    //Then there is obstacle here
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPos, GridPosition endGridPos)
    {
        HashSet<PathNode> openList = new HashSet<PathNode>();
        HashSet<PathNode> closedList = new HashSet<PathNode>();
        
        PathNode startNode = _gridSystem.GetGridObject(startGridPos);
        PathNode endNode = _gridSystem.GetGridObject(endGridPos);
        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                var gridPos = new GridPosition(x, z);
                PathNode curNode = _gridSystem.GetGridObject(gridPos);

                curNode.SetGCost(int.MaxValue);
                curNode.SetHCost(0);
                curNode.CalculateFCost();
                curNode.ResetCameFromPathNode();

            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPos, endGridPos));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode curNode = GetLowestFCostPathNode(openList);

            if (curNode == endNode)
            {
                //Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(curNode);
            closedList.Add(curNode);

            foreach (PathNode neighborNode in GetNeighborList(curNode))
            {
                if (closedList.Contains(neighborNode))
                {
                    continue;
                }

                if (!neighborNode.IsWalkable())
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = curNode.GetGCost() + CalculateDistance(curNode.GetGridPosition(), neighborNode.GetGridPosition());

                if (tentativeGCost < neighborNode.GetGCost())
                {
                    neighborNode.SetCameFromPathNode(curNode);
                    neighborNode.SetGCost(tentativeGCost);
                    neighborNode.SetHCost(CalculateDistance(neighborNode.GetGridPosition(), endGridPos));
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        
        //No path found
        return null;

    }
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        //List<PathNode> pathNodeList = new List<PathNode>();
        Stack<PathNode> pathNodeList = new Stack<PathNode>();
        pathNodeList.Push(endNode);

        PathNode curNode = endNode;

        while (curNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Push(curNode.GetCameFromPathNode());
            curNode = curNode.GetCameFromPathNode();
        }

        //pathNodeList.Reverse();
        List<GridPosition> gridPositonList = new List<GridPosition>();
        while (pathNodeList.Count > 0)
        {
            gridPositonList.Add(pathNodeList.Pop().GetGridPosition());    
        }
        // foreach (PathNode pathNode in pathNodeList)
        // {
        //     gridPositonList.Add(pathNode.GetGridPosition());
        // }

        return gridPositonList;
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a - b;
        int totalDistance = Mathf.Abs(gridPositionDistance.X) + Mathf.Abs(gridPositionDistance.Z);
        int xDistance = Mathf.Abs(gridPositionDistance.X);
        int zDistance = Mathf.Abs(gridPositionDistance.Z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(HashSet<PathNode> pathNodeList)
    {
        List<PathNode> pathNodes = pathNodeList.ToList();
        PathNode lowestFCostNode = pathNodes[0];

        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].GetFCost() < lowestFCostNode.GetFCost())
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z));
    }
    
    private List<PathNode> GetNeighborList(PathNode curNode)
    {
        List<PathNode> neighbors = new List<PathNode>();

        GridPosition gridPos = curNode.GetGridPosition();

        if (gridPos.X - 1 >= 0)
        {
            //Left
            neighbors.Add(GetNode(gridPos.X - 1, gridPos.Z));
            if (gridPos.Z - 1 >= 0)
            {
                //Left Down
                neighbors.Add(GetNode(gridPos.X - 1, gridPos.Z - 1));
            }
            if (gridPos.Z + 1< _gridSystem.GetHeight())
            {
                //Left Up
                neighbors.Add(GetNode(gridPos.X - 1, gridPos.Z + 1));
            }
        }
        if (gridPos.X + 1 < _gridSystem.GetWidth())
        {
            //Right
            neighbors.Add(GetNode(gridPos.X + 1, gridPos.Z));
            if (gridPos.Z - 1 >= 0)
            {
                //Right Down
                neighbors.Add(GetNode(gridPos.X + 1, gridPos.Z - 1));
            }
            if (gridPos.Z  + 1 < _gridSystem.GetHeight())
            {
                //Right Up
                neighbors.Add(GetNode(gridPos.X + 1, gridPos.Z + 1));
            }
        }
        if (gridPos.Z - 1 >= 0)
        {
            //Down
            neighbors.Add(GetNode(gridPos.X, gridPos.Z - 1));
        }
        if (gridPos.Z + 1 < _gridSystem.GetHeight())
        {
            //Up
            neighbors.Add(GetNode(gridPos.X , gridPos.Z + 1));
        }
        
        return neighbors;
    }

}
