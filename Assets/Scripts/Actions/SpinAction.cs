using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction 
{
    
    private float _totalSpinAmount;
   
    private void Update()
    {
        if (!_isActive) return;
        
        float spinAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        _totalSpinAmount += spinAmount;
        if (_totalSpinAmount >= 360f)
        {
           ActionComplete();
        }

    }

    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        _totalSpinAmount = 0;
        ActionStart(onSpinComplete);
    }
    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        //List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
