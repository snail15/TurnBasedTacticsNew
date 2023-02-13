using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {
    
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool _isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update()
    {
        if (_isBusy) return;
        
        if (Input.GetMouseButton(0))
        {
            if (TryHandleUnitSelection()) return;

            GridPosition mouseGridPos = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            
            if(selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPos))
            {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPos, ClearBusy);
            }
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
    }

    private void ClearBusy()
    {
        _isBusy = false;
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycast, float.MaxValue, unitLayerMask))
        {
            if (raycast.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
