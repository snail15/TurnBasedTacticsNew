using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {
   private Vector3 _targetPosition;
   [SerializeField] private TrailRenderer trailRenderer;
   [SerializeField] private Transform bulletHitVFXPrefab;
   public void SetUp(Vector3 targetPosition)
   {
      _targetPosition = targetPosition;
   }

   private void Update()
   {
      Vector3 moveDir = (_targetPosition - transform.position).normalized;

      float distanceBeforeMoving = Vector3.Distance(_targetPosition, transform.position);
      
      float moveSpeed = 200f;
      transform.position += moveDir * moveSpeed * Time.deltaTime;
      
      float distanceAfterMoving = Vector3.Distance(_targetPosition, transform.position);

      if (distanceBeforeMoving < distanceAfterMoving)
      {
         transform.position = transform.position;
         trailRenderer.transform.parent = null;
         Destroy(gameObject);

         Instantiate(bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
      }
   }
}
