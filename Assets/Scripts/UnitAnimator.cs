using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour 
{
   [SerializeField] private Animator animator;
   [SerializeField] private Transform bulletProjectilePrefab;
   [SerializeField] private Transform shootPointTransform;
   
   private static readonly int IsWalking = Animator.StringToHash("IsWalking");
   private static readonly int Shoot = Animator.StringToHash("Shoot");

   private void Awake()
   {
      if (TryGetComponent<MoveAction>(out MoveAction moveAction))
      {
         moveAction.OnStartMoving += OnStartMoving;
         moveAction.OnStopMoving += OnStopMoving;
      }
      if (TryGetComponent<ShootAction>(out ShootAction shootAction))
      {
         shootAction.OnShoot += OnShoot;
         moveAction.OnStopMoving += OnStopMoving;
      }
   }
   private void OnShoot(object sender, ShootAction.OnShootEventArgs e)
   {
      animator.SetTrigger(Shoot);
      
      Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
      BulletProjectile bulletProjectile =bulletProjectileTransform.GetComponent<BulletProjectile>();

      Vector3 targetUnitShootPosition = e.targetUnit.GetUnitWorldPosition();
      targetUnitShootPosition.y = shootPointTransform.position.y; // to fix shooting feet issue
      bulletProjectile.SetUp(targetUnitShootPosition);


   }
   private void OnStopMoving(object sender, EventArgs e)
   {
      animator.SetBool(IsWalking, false);
   }
   private void OnStartMoving(object sender, EventArgs e)
   {
      animator.SetBool(IsWalking, true);
   }
   
   
}

