using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour {

   [SerializeField] private Transform actionButtonPrefab;
   [SerializeField] private Transform actionButtonContainerTransform;
   [SerializeField] private TextMeshProUGUI actionPointsText;
   
   private List<ActionButtonUI> _actionButtonUIs;

   private void Awake()
   {
      _actionButtonUIs = new List<ActionButtonUI>();
   }
   private void Start()
   {
      UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
      UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
      UnitActionSystem.Instance.OnActionStarted += OnActionStarted;
      CreateUnitActionButtons();
      UpdatedSelectedVisual();
      UpdateActionPoints();
   }
   private void OnActionStarted(object sender, EventArgs e)
   {
      UpdateActionPoints();
   }
   private void OnSelectedActionChanged(object sender, EventArgs e)
   {
      UpdatedSelectedVisual();
      UpdateActionPoints();
   }
   private void OnSelectedUnitChanged(object sender, EventArgs e)
   {
      CreateUnitActionButtons();
      UpdatedSelectedVisual();
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

         _actionButtonUIs.Add(actionButtonUI);
      }
   }

   private void ClearExistingActionButtons()
   {
      foreach (Transform buttonTransform in actionButtonContainerTransform)
      {
         Destroy(buttonTransform.gameObject);
      }
      
      _actionButtonUIs.Clear();
   }

   private void UpdatedSelectedVisual()
   {
      foreach (ActionButtonUI actionButtonUI in _actionButtonUIs)
      {
         actionButtonUI.UpdatedSelectedVisual();
      }
   }

   private void UpdateActionPoints()
   {
      actionPointsText.text = $"Action Points: {UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints()}";
   }
}
