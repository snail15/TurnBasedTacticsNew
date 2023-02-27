using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class MoveAction : BaseAction 
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 _targetPos;
    
    protected override void Awake()
    {
        base.Awake();
        _targetPos = transform.position;
    }
    

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        float stoppingdDistance = 0.1f;
        Vector3 moveDir = (_targetPos - transform.position).normalized;
        if (Vector3.Distance(transform.position, _targetPos) > stoppingdDistance)
        {
            
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

           
        }
        else
        {
            
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
        
        //Rotate
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public override void TakeAction(GridPosition targetPos, Action onMoveComplete)
    {
        _targetPos = LevelGrid.Instance.GetWorldPosition(targetPos);
        
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
        int targetCountAtGridPosition = _unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        
        return new EnemyAIAction
        {
            gridPosition =  gridPosition,
            actionValue = targetCountAtGridPosition * 10
        };
    }

   
}
