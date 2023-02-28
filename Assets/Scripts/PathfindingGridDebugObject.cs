using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PathfindingGridDebugObject : GridDebugObject 
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;

    private PathNode _pathNode;
    
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        _pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = _pathNode.GetGCost().ToString();
        hCostText.text = _pathNode.GetHCost().ToString();
        fCostText.text = _pathNode.GetFCost().ToString();
    }

}
