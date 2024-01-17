using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Battle_AI_Brain : MonoBehaviour
{
    //Choosing which Action is the best. Calculating out of each consideration in each action.
    
    public bool finishedDeciding { get; set; }
    public bool finishedExecutingBestAction { get; set; }
    
    public bool attackFinishedDeciding { get; set; }
    public bool attackFinishedExecutingBestAction { get; set; }

    private Battle_AI_Controller aiNpc;
    public Battle_AI_Action bestAction { get; set; }

    [SerializeField] private Battle_AI_Action[] actionsAvailable;

    [SerializeField] private Battle_AI_Action[] attackActionsAvailable;

    private void Awake()
    {
        aiNpc = GetComponent<Battle_AI_Controller>();
        finishedDeciding = false;
        finishedExecutingBestAction = false;

        attackFinishedDeciding = false;
        attackFinishedExecutingBestAction = false;
    }

    public void DecideBestAction()
    {
        finishedExecutingBestAction = false;
        float score = 0f;
        int nextBestActionIndex = 0;

        for (int i = 0; i < actionsAvailable.Length; i++)
        {
            //Debug.Log("Diese Action liste: " + actionsAvailable[i].name);
            if (ScoreAction(actionsAvailable[i]) > score)
            {
                nextBestActionIndex = i;
                score = actionsAvailable[i].score;
            }
        }
        
        bestAction = actionsAvailable[nextBestActionIndex];
        //Debug.Log("Beste Action: " + bestAction.name + " + " + aiNpc.gameObject.name);
        bestAction.SetRequiredDestination(aiNpc);
        finishedDeciding = true;
    }

    public float ScoreAction(Battle_AI_Action action)
    {
        float score = 1f;
        for (int i = 0; i < action.considerations.Length; i++)
        {
            float considerationScore = action.considerations[i].ScoreConsideration(aiNpc);
            score *= considerationScore;
            if (score == 0)
            {
                
                action.score = 0;
                return action.score;
            }
        }

        float originalScore = score;
        float modFactor = 1 - (1 / action.considerations.Length);
        float makeupValue = (1 / originalScore) * modFactor;
        action.score = originalScore + (makeupValue * originalScore);
        //Debug.Log( "Current Index: "+ BattleRoundManager.battleRoundCounter +" "+"Score: " + action.score + " Name: " + action.name + " Unit Name: " + aiNpc.gameObject.name);
        return action.score;
    }

    public void DecideBestAttack()
    {
        attackFinishedExecutingBestAction = false;
        float score = 0f;
        int nextBextActionIndex = 0;

        for (int i = 0; i < attackActionsAvailable.Length; i++)
        {
            if (ScoreAction(attackActionsAvailable[i]) > score)
            {
                nextBextActionIndex = i;
                score = attackActionsAvailable[i].score;
            }
        }

        bestAction = attackActionsAvailable[nextBextActionIndex];
        bestAction.AttackRequiredPlayer(aiNpc);
        attackFinishedDeciding = true;
    }
}
