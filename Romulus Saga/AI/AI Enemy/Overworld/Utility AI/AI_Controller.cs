using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public enum UtilityAIState
{
    decide,
    move,
    execute, 
    failed
}
public class AI_Controller : MonoBehaviour
{
    
    //Responsible for applying the actions that are best for him
    public AI_Brain aiBrain { get; set; }
    public AI_StorageInventory baseInventory { get; set; }
    public UtilityAIState currenState { get; set; }

    private bool getCalledOnce = false;
    public int counter;
    public int idleOverTime = 100;

    private bool deactivateFSM = true;
    
    // Start is called before the first frame update
    void Start()
    {
        aiBrain = GetComponent<AI_Brain>();
        baseInventory = GetComponent<AI_StorageInventory>();
        currenState = UtilityAIState.decide;
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenuController.instance.currentGameState == GameState.Paused)
            return;
        if (!deactivateFSM)
        {
            FSMTick();
        }
        else if (deactivateFSM && !getCalledOnce)
        {
            StartCoroutine(BuildBuilding());
            getCalledOnce = true;
        }
    }

    public void FSMTick()
    {
        if (currenState == UtilityAIState.decide)
        {
            aiBrain.DecideBestAction();
            aiBrain.failedToExecuteBestAction = false;
            currenState = UtilityAIState.execute;
        }
        else if (currenState == UtilityAIState.move)
        {
            if (aiBrain.failedToExecuteBestAction)
                currenState = UtilityAIState.failed;
            else
                currenState = UtilityAIState.execute;
        }
        else if (currenState == UtilityAIState.execute)
        {
            if (aiBrain.failedToExecuteBestAction)
                currenState = UtilityAIState.failed;
            else
            {
                if (!aiBrain.finishedExecutingBestAction)
                {
                    aiBrain.bestAction.Execute(this);
                    aiBrain.bestAction.CreateBuilding(this);
                }
                else if (aiBrain.finishedExecutingBestAction)
                {
                    currenState = UtilityAIState.decide; 
                }
            }
            deactivateFSM = true;
            getCalledOnce = false;
        }
        else if (currenState == UtilityAIState.failed)
        {
            currenState = UtilityAIState.decide;
        }
    }

    public void OnFinishedAction()
    {
        aiBrain.DecideBestAction();
    }

    IEnumerator BuildBuilding()
    {
        yield return new WaitForSecondsRealtime(120f);
        deactivateFSM = false;
    }

}
