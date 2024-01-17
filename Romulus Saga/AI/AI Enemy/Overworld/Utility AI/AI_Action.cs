using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Action : ScriptableObject
{
    //Im Inspector + Script eine Action erstellen, die die AI ausführen kann + die Gewichtung der Action
    public string name;
    private float _score;

    public float score
    {
        get { return _score; }
        set
        {
            this._score = Mathf.Clamp01(value);
        }
    }

    public AI_Consideration[] considerations;
    
    //public Transform RequiredDestination { get; protected set; }

    public virtual void Awake()
    {
        score = 0;
    }

    public abstract void Execute(AI_Controller aiBase);
    
    //public virtual void SetRequiredDestination(AI_Controller aiBase){}

    public virtual void CreateBuilding(AI_Controller aiBase) {}
}
