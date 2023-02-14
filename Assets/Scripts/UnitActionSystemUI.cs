using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour {

   [SerializeField] private Transform actionButtonPrefab;
   [SerializeField] private Transform actionButtonContainerTransform;
   
   private void Start()
   {
      UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
      CreateUnitActionButtons();
   }
   private void OnSelectedUnitChanged(object sender, EventArgs e)
   {
      CreateUnitActionButtons();
   }

   private void CreateUnitActionButtons()
   {
      ClearExistingActionButtons();
      
      Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

      foreach (BaseAction baseAction in selectedUnit.GetBaseActions())
      {
         Transform actionButton = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
         var actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
         actionButtonUI.SetBaseAction(baseAction);
      }
   }

   private void ClearExistingActionButtons()
   {
      foreach (Transform buttonTransform in actionButtonContainerTransform)
      {
         Destroy(buttonTransform.gameObject);
      }
   }
}
