using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle_AI_Action : ScriptableObject
{
    // Actions which move a unit can choose from : ScritableObject.
    
    public string name;
    private float _score;
    
    public float score
    {
        get { return _score; }
        set { this._score = Mathf.Clamp01(value); }
    }

    public Battle_AI_Consideration[] considerations;
    public HexaTile RequiredDestination { get; protected set; }
    private void Awake()
    {
        score = 0;
    }

    public abstract void Execute(Battle_AI_Controller aiNpc);

    public virtual void SetRequiredDestination(Battle_AI_Controller aiNpc){}
    
    public virtual void AttackRequiredPlayer(Battle_AI_Controller aiNpc){}

}
