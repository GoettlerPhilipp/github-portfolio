using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle_AI_Consideration : ScriptableObject
{
    // Calculate best scenario component :  ScriptableObject
    
    public string name;

    private float _score;
    
    public float score
    {
        get { return _score; }
        set { this._score = Mathf.Clamp01(value); }
    }

    private void Awake()
    {
        score = 0;
    }

    public abstract float ScoreConsideration(Battle_AI_Controller aiNpc);
}
