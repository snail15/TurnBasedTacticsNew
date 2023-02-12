using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction {
    
    private float _totalSpinAmount;
    private void Update()
    {
        if (!_isActive) return;
        
        float spinAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        _totalSpinAmount += spinAmount;
        if (_totalSpinAmount >= 360f)
        {
            _isActive = false;
        }

    }

    public void Spin()
    {
        _isActive = true;
        _totalSpinAmount = 0;
    }
}
