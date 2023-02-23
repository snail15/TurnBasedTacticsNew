using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour 
{
   [SerializeField] private Animator animator;
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
   private void OnShoot(object sender, EventArgs e)
   {
      animator.SetTrigger(Shoot);
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

