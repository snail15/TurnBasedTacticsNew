using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
   private List<Unit> _unitList;
   private List<Unit> _friendlyUnitList;
   private List<Unit> _enemyUnitList;

   protected override void Awake()
   {
      base.Awake();
      _unitList = new List<Unit>();
      _friendlyUnitList = new List<Unit>();
      _enemyUnitList = new List<Unit>();
   }
   private void Start()
   {
      Unit.OnAnyUnitSpawned += OnAnyUnitSpawned;
      Unit.OnAnyUnitDead += OnAnyUnitDead;
   }
   private void OnAnyUnitDead(object sender, EventArgs e)
   {
      Unit unit = sender as Unit;
      if (unit!.IsEnemy())
      {
         _enemyUnitList.Remove(unit);
      }
      else
      {
         _friendlyUnitList.Remove(unit);
      }

      _unitList.Remove(unit);
   }
   private void OnAnyUnitSpawned(object sender, EventArgs e)
   {
      Unit unit = sender as Unit;
      if (unit!.IsEnemy())
      {
         _enemyUnitList.Add(unit);
      }
      else
      {
         _friendlyUnitList.Add(unit);
      }

      _unitList.Add(unit);

   }

   public List<Unit> GetUnitList()
   {
      return _unitList;
   }
   public List<Unit> GetFriendlyUnitList()
   {
      return _friendlyUnitList;
   }
   public List<Unit> GetEnemyUnitList()
   {
      return _enemyUnitList;
   }
}
