using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour 
{
   public static GridSystemVisual Instance { get; private set; }
   
   [SerializeField] private Transform gridSystemVisualSinglePrefab;

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
   }

   private void Update()
   {
      UpdateGridVisual();
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

   public void ShowGridPositionList(List<GridPosition> gridPositions)
   {
      foreach (GridPosition gridPosition in gridPositions)
      {
         _gridSystemVisualSingles[gridPosition.X, gridPosition.Z].Show();
      }
   }

   private void UpdateGridVisual()
   {
      HideAllGridPosition();
      BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
      ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
   }
}
