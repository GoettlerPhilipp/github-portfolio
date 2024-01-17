using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IdleAction", menuName = "UtilityAI/AI_Actions/IdleAction")]
public class IdleAction : AI_Action
{
    public override void Execute(AI_Controller aiBase)
    {
        aiBase.counter = 1;
        if(aiBase.idleOverTime >= 0)
            aiBase.idleOverTime -= 10;
        Debug.Log(aiBase.idleOverTime);
        Debug.Log("Idle Action");
        aiBase.aiBrain.finishedExecutingBestAction = true;
    }
}
