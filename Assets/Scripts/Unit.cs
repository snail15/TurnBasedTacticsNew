using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] private Animator _animator;
    private Vector3 _targetPos;
    private GridPosition _gridPosition;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _targetPos = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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
        
        //Update Grid Position
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPos)
    {
        _targetPos = targetPos;
    }
}
