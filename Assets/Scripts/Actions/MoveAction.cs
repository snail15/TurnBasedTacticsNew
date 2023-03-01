using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEngine;

public class MoveAction : BaseAction 
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    
    [SerializeField] private int maxMoveDistance = 4;
    private List<Vector3> _targetPosList;
    private int _currentPosIdx;
    
    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        Vector3 targetPos = _targetPosList[_currentPosIdx];
        float stoppingdDistance = 0.1f;
        Vector3 moveDir = (targetPos - transform.position).normalized;
        
        //Rotate
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        
        if (Vector3.Distance(transform.position, targetPos) > stoppingdDistance)
        {
            
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

           
        }
        else
        {
            _currentPosIdx += 1;
            if (_currentPosIdx >= _targetPosList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
            
        }
        
       
    }

    public override void TakeAction(GridPosition gridPosition, Action onMoveComplete)
    {
        List<GridPosition> pathGridPositionList =  Pathfinding.Instance.FindPath(_unit.GetGridPosition(), gridPosition, out int pathLength);
        
        _currentPosIdx = 0;
        _targetPosList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
           _targetPosList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        
        ActionStart(onMoveComplete);
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        var unitGridPos = _unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPos + offsetGridPos;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPos))
                {
                    continue;
                }
                if (unitGridPos == testGridPos)
                {
                    continue;
                }
                if (LevelGrid.Instance.HasUnitOnGridPosition(testGridPos))
                {
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPos))
                {
                    continue;
                }
                if (!Pathfinding.Instance.HasPath(unitGridPos, testGridPos))
                {
                    continue;
                }

                int pathFindingDistanceMultiple = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPos, testGridPos) > maxMoveDistance * pathFindingDistanceMultiple)
                {
                    //Path length too long
                    continue;
                }
                
                validGridPositionList.Add(testGridPos);
            }
        }

        return validGridPositionList;
    }
    
    public override string GetActionName()
    {
        return "Move";
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = _unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        
        return new EnemyAIAction
        {
            gridPosition =  gridPosition,
            actionValue = targetCountAtGridPosition * 10
        };
    }

   
}
