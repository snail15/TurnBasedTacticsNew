using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour 
{
    
    protected Unit _unit;
    protected bool _isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    protected void ActionStart(Action onActionComplete)
    {
        _isActive = true;
        this.onActionComplete = onActionComplete;
    }

    protected void ActionComplete()
    {
        _isActive = false;
        onActionComplete();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPosList = GetValidActionGridPositionList();
        return validGridPosList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }
}
