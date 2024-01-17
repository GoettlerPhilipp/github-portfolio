using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Consideration : ScriptableObject
{
    //Bediengungen für die Actions
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

    public virtual void Awake()
    {
        score = 0;
    }

    public abstract float ScoreConsideration(AI_Controller aiBase);
}
