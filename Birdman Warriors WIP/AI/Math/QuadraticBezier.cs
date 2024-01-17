using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuadraticBezier : BezierBase
{
    public override void GetBezier(out Vector3 pos, List<Vector3> _Checkpoints, float time)
    {
        _Checkpoints = thisObject.GetComponent<Comet>().currentList;
        if (_Checkpoints.Count > 3)
            for (int i = 3; i < _Checkpoints.Count; i++)
                _Checkpoints.RemoveAt(i);
            
        
			
        QuadraticBezierEquation.GetCurve(out pos,
            _Checkpoints[0],
            _Checkpoints[1],
            _Checkpoints[2],
            time);
    }

    private void Update()
    {
        GetBezier(out myPosition, Checkpoints, time);
        this.transform.position = myPosition;
    }
}
