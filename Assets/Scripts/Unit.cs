using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    private Vector3 _targetPos;

    private void Update()
    {
        float stoppingdDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPos) > stoppingdDistance)
        {
            Vector3 moveDir = (_targetPos - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4,0,4));
        }
    }

    private void Move(Vector3 targetPos)
    {
        _targetPos = targetPos;
    }
}
