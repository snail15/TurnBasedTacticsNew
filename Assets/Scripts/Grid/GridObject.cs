using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GridObject {
  

   private GridSystem<GridObject> _gridSystem;
   private GridPosition _gridPosition;
   private List<Unit> _units;
   
   public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
   {
      _gridSystem = gridSystem;
      _gridPosition = gridPosition;
      _units = new List<Unit>();
   }

   public override string ToString()
   {
      StringBuilder sb = new StringBuilder();
      foreach (Unit unit in _units)
      {
         sb.Append(unit + "\n");
      }
      return _gridPosition.ToString() + "\n" + sb.ToString();
   }

   public void AddUnit(Unit unit)
   {
      _units.Add(unit);;
   }

   public List<Unit> GetUnitList()
   {
      return _units;
   }

   public void RemoveUnit(Unit unit)
   {
      _units.Remove(unit);
   }

   public bool HasAnyUnit()
   {
      return _units.Count > 0;
   }

   public Unit GetUnit()
   {
      if (HasAnyUnit())
      {
         return _units[0];
      }
      else
      {
         return null;
      }
   }
   
}
