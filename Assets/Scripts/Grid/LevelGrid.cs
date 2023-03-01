using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
   public static LevelGrid Instance { get; private set; }

   public event EventHandler OnAnyUnitMovedGridPosition;
   
   [SerializeField] private Transform gridDebugObject;
   [SerializeField] private int width;
   [SerializeField] private int height;
   [SerializeField] private float cellSize;
   
   private GridSystem<GridObject> _gridSystem;
   
   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("There is more than one intance LevelGrid");
         Destroy(gameObject);
         return;
      }
      Instance = this;
      _gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
      //_gridSystem.CreateDebugObjects(gridDebugObject);
   }

   private void Start()
   {
      Pathfinding.Instance.Setup(width, height, cellSize);
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
      OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
   }
   public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
   public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
   public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
   public int GetWidth() => _gridSystem.GetWidth();
   public int GetHeight() => _gridSystem.GetHeight();

   public bool HasUnitOnGridPosition(GridPosition gridPosition)
   {
      var gridObject = _gridSystem.GetGridObject(gridPosition);
      return gridObject.HasAnyUnit();
   }

   public Unit GetUnitAtGridPosition(GridPosition gridPosition)
   {
      var gridObject = _gridSystem.GetGridObject(gridPosition);
      return gridObject.GetUnit();
   }
   
   
}
