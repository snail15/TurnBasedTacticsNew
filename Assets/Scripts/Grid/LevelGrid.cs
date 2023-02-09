using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
   public static LevelGrid Instance { get; private set; }
   
   [SerializeField] private Transform gridDebugObject;
   private GridSystem _gridSystem;
   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("There is more than one intance LevelGrid");
         Destroy(gameObject);
         return;
      }
      Instance = this;
      _gridSystem = new GridSystem(10, 10, 2f);
      _gridSystem.CreateDebugObjects(gridDebugObject);
   }

   public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
   {
      GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
      gridObject.AddUnit(unit);
      
   }

   public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
   {
      GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
      return gridObject.GetUnitList();
   }

   public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
   {
      GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
      gridObject.RemoveUnit(unit);
   }

   public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
   {
      RemoveUnitAtGridPosition(fromGridPosition, unit);
      AddUnitAtGridPosition(toGridPosition, unit);
   }
   public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
   public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
   public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

   public bool HasUnitOnGridPosition(GridPosition gridPosition)
   {
      var gridObject = _gridSystem.GetGridObject(gridPosition);
      return gridObject.HasUnit();
   }
}
