using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : Singleton<TurnSystem>
{
   private int _turnNumber = 1;
   private bool _isPlayerTurn = true;

   public event EventHandler OnTurnChanged;

   public void NextTurn()
   {
      _turnNumber += 1;
      _isPlayerTurn = !_isPlayerTurn;
      OnTurnChanged?.Invoke(this, EventArgs.Empty);
   }

   public int GetTurnNumber()
   {
      return _turnNumber;
   }

   public bool IsPlayerTurn()
   {
      return _isPlayerTurn;
   }
}
