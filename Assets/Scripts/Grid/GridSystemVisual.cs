using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour 
{
   public static GridSystemVisual Instance { get; private set; }

   [Serializable]
   public struct GridVisualTypeMaterial 
   {
      public GridVisualType gridVisualType;
      public Material material;
   }
   public enum GridVisualType
   {
      White,
      Blue,
      Red,
      Yellow,
      RedSoft,
   }
   
   [SerializeField] private Transform gridSystemVisualSinglePrefab;
   [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

   private GridSystemVisualSingle[,] _gridSystemVisualSingles;

   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("GridSystemVisual singleton already exists");
         Destroy(gameObject);
         return;
      }
      Instance = this;
   }

   private void Start()
   {
      _gridSystemVisualSingles = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
      for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
      {
         for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
         {
            var gridPos = new GridPosition(x, z);
            Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPos), Quaternion.identity);
            _gridSystemVisualSingles[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
         }
      }
      
      UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
      LevelGrid.Instance.OnAnyUnitMovedGridPosition += OnAnyUnitMovedGridPosition;
      
      UpdateGridVisual();
   }
   private void OnAnyUnitMovedGridPosition(object sender, EventArgs e)
   {
      UpdateGridVisual();
   }
   private void OnSelectedActionChanged(object sender, EventArgs e)
   {
      UpdateGridVisual();
   }

   private void Update()
   {
      //UpdateGridVisual();
   }

   public void HideAllGridPosition()
   {
      for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
      {
         for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
         {
            _gridSystemVisualSingles[x, z].Hide();
         }
      }
   }

   private void ShowGridPositionRange(GridPosition gridPos, int range, GridVisualType gridVisualType)
   {
      List<GridPosition> gridPositionList = new List<GridPosition>();
      
      for (int x = -range; x <= range; x++)
      {
         for (int z = -range; z <= range; z++)
         {
            GridPosition testGridPos = gridPos + new GridPosition(x, z);

            if (!LevelGrid.Instance.IsValidGridPosition(testGridPos))
            {
               continue;
            }
            
            int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
            if (testDistance > range) // Out of range
            {
               continue;
            }
            
            gridPositionList.Add(testGridPos);
            
         }
      }
      
      ShowGridPositionList(gridPositionList, gridVisualType);
   }
   public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
   {
      foreach (GridPosition gridPosition in gridPositions)
      {
         _gridSystemVisualSingles[gridPosition.X, gridPosition.Z].Show(GetGridVisualTypeMaterial(gridVisualType));
      }
   }

   private void UpdateGridVisual()
   {
      HideAllGridPosition();
      Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
      BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
      GridVisualType gridVisualType = GridVisualType.White;
      switch (selectedAction)
      {
         case MoveAction moveAction:
            gridVisualType = GridVisualType.White;
            break;
         case SpinAction spinAction:
            gridVisualType = GridVisualType.Blue;
            break;
         case ShootAction shootAction:
            gridVisualType = GridVisualType.Red;
            ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
            break;
      }
      ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
   }

   private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
   {
      foreach (GridVisualTypeMaterial material in gridVisualTypeMaterialList)
      {
         if (material.gridVisualType == gridVisualType)
         {
            return material.material;
         }
      }

      throw new ArgumentException("There is no grid visual type");
   }
}
