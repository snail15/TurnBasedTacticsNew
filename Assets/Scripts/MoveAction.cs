using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 _targetPos;
    private Unit _unit;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _targetPos = transform.position;
    }

    private void Update()
    {
        float stoppingdDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPos) > stoppingdDistance)
        {
            Vector3 moveDir = (_targetPos - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            
            //Rotate
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
            
            _animator.SetBool(IsWalking, true);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
        }
    }

    public void Move(Vector3 targetPos)
    {
        _targetPos = targetPos;
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
            }
        }

        return validGridPositionList;
    }
}
