using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction {

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs 
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    
    private enum State 
    {
        Aiming,
        Shooting,
        CoolOff
    }
    private float _totalSpinAmount;
    private int _maxShootDistance = 7;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;
    
    private void Update()
    {
        if (!_isActive) return;

        _stateTimer -= Time.deltaTime;
        switch (_state)
        {
            case State.Aiming:
                Vector3 aimDir = (_targetUnit.GetUnitWorldPosition() - _unit.GetUnitWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.CoolOff:
             
                break;
        }

        if (_stateTimer <= 0f)
        {
            NextState();
        }

    }
    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs{ targetUnit = _targetUnit, shootingUnit = _unit});
        _targetUnit.Damage(40);
    }
    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTime = 0.1f;
                _stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                _state = State.CoolOff;
                float coolOffStateTime = 0.5f;
                _stateTimer = coolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }
        
        
    }

    public override string GetActionName()
    {
        return "Shoot";
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        _state = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;

        _canShootBullet = true;

        ActionStart(onActionComplete);
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        var unitGridPos = _unit.GetGridPosition();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPos + offsetGridPos;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPos))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootDistance) // Out of range
                {
                    continue;
                }

                if (unitGridPos == testGridPos)
                {
                    continue;
                }
                if (!LevelGrid.Instance.HasUnitOnGridPosition(testGridPos))
                {
                    //Grid position is empty - no unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPos);

                if (targetUnit.IsEnemy() == _unit.IsEnemy()) //both are on the same team
                {
                    continue;
                }
                
                validGridPositionList.Add(testGridPos);
            }
        }

        return validGridPositionList;
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return _maxShootDistance;
    }
}
