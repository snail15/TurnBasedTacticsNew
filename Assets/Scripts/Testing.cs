using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

   [SerializeField] private Unit unit;
   private void Start()
   {
     
   }
   private void Update()
   {
      // if (Input.GetKeyDown(KeyCode.T))
      // {
      //    GridPosition mouseGridPos = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
      //    GridPosition startGridPos = new GridPosition(0, 0);
      //
      //    List<GridPosition> gridPositionList =  Pathfinding.Instance.FindPath(startGridPos, mouseGridPos);
      //
      //    for (int i = 0; i < gridPositionList.Count - 1; i++)
      //    {
      //       Debug.DrawLine(
      //          LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
      //          LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
      //          Color.white,
      //          10f
      //          );
      //    }
      //    
      // }
   }
}
