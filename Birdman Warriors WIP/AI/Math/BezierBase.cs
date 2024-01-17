using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BezierBase : MonoBehaviour
{
    [Range(0f, 1f)] 
    [SerializeField] protected float time;
    [SerializeField] protected bool AutoTime;
    [Range(0.1f, 10f)] 
    [SerializeField] protected float timeScale;
    [SerializeField] protected List<Vector3> Checkpoints = new List<Vector3>();
    [SerializeField] protected GameObject thisObject;

    protected Vector3 myPosition;

    public abstract void GetBezier(out Vector3 pos, List<Vector3> _Checkpoints, float time);

    private IEnumerator Start()
    {
        thisObject = gameObject;
        while (true)
        {
            if (AutoTime)
            {
                time += Time.deltaTime * timeScale;

                if (timeScale <= 0f)
                    timeScale = 0.1f;

                if (time >= 1f)
                    time = 0f;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

