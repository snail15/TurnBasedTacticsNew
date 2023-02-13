using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 _targetPos;
 
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    protected override void Awake()
    {
        base.Awake();
        _targetPos = transform.position;
    }

    private void Update()
    {
        if (!_isActive) return;
        
        float stoppingdDistance = 0.1f;
        Vector3 moveDir = (_targetPos - transform.position).normalized;
        if (Vector3.Distance(transform.position, _targetPos) > stoppingdDistance)
        {
            
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            _animator.SetBool(IsWalking, true);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
            _isActive = false;
            onActionComplete();
        }
        
        //Rotate
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public void Move(GridPosition targetPos, Action onMoveComplete)
    {
        this.onActionComplete = onMoveComplete;
        _targetPos = LevelGrid.Instance.GetWorldPosition(targetPos);
        _isActive = true;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPosList = GetValidActionGridPositionList();
        return validGridPosList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
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
}
