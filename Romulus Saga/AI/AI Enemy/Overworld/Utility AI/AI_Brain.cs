using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AI_Brain : MonoBehaviour
{
    //Choosing which Action is the best for the next building to build. Calculating out of each consideration in each action.
    
    
    public bool finishedDeciding { get; set; }
    public bool finishedExecutingBestAction { get; set; }
    public bool failedToExecuteBestAction { get; set; }

    
    public AI_Action bestAction { get; set; }
    private AI_Controller aiBase;
    
    [SerializeField] private AI_Action[] actionsAvailable;


    private void Start()
    {
        aiBase = GetComponent<AI_Controller>();
        finishedDeciding = false;
        finishedExecutingBestAction = false;
        failedToExecuteBestAction = false;
    }

    public void DecideBestAction()
    {
        finishedExecutingBestAction = false;
        
        float score = 0f;
        int nextBestActionIndex = 0;

        for (int i = 0; i < actionsAvailable.Length; i++)
        {
            
            if (ScoreAction(actionsAvailable[i]) > score)
            {
                nextBestActionIndex = i;
                score = actionsAvailable[i].score;
                
            }
        }

        bestAction = actionsAvailable[nextBestActionIndex];
        bestAction.CreateBuilding(aiBase);

        finishedDeciding = true;
    }

    public float ScoreAction(AI_Action action)
    {
        float score = 1f;
        for (int i = 0; i < action.considerations.Length; i++)
        {
            float considerationScore = action.considerations[i].ScoreConsideration(aiBase);
            //Debug.Log(action.considerations[i].name + " + " + action.considerations[i].score.ToString("F3"));
            score *= considerationScore;
            if (score == 0)
            {
                action.score = 0;
                return action.score;
            }
        }

        float originalScore = score;
        float modFactor = 1 - (1 / action.considerations.Length);
        float makeupValue = (1 - originalScore) * modFactor;
        action.score = originalScore + (makeupValue * originalScore);
        //Debug.Log(action.name + " + " + action.score + " + Original Score: " + originalScore + " + Mod Factor: " + modFactor + " + Makeup Value: " + makeupValue);
        return action.score;
    }
}
