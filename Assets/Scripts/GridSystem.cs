using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
   
   private int _width;
   private int _height;
   private float _cellSize;
   public GridSystem(int width, int height, float cellSize)
   {
      _width = width;
      _height = height;
      _cellSize = cellSize;

      for (int x = 0; x < _width; x++)
      {
         for (int z = 0; z < _height; z++)
         {
            Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * 0.2f, Color.white, 1000);
         }
      }
   }

   public Vector3 GetWorldPosition(int x, int z)
   {
      return new Vector3(x, 0, z) * _cellSize;
   }

   public GridPosition GetGridPosition(Vector3 worldPosition)
   {
      return new GridPosition(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
   }
}
