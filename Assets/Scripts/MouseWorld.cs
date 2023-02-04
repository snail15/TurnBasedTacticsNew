using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour {
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        //Physics.Raycast(ray);
    }
}
