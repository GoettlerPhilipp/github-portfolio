using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle_MoveToPositionCenter", menuName = "UtilityAI/Battle_AI/MoveToPositionCenter")]
public class Battle_MoveToPositionCenter : Battle_AI_Action
{
    public override void Execute(Battle_AI_Controller aiNpc)
    {
        Debug.Log("Execute MoveToPosition");
        aiNpc.thisUnit.aiBrain.finishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination(Battle_AI_Controller aiNpc)
    {
        aiNpc.thisUnit.RemoveOccupiedTiles();
        aiNpc.thisUnit.GetBestCenterTile();
        aiNpc.thisUnit.MoveToDestination();
        aiNpc.thisUnit.confirmMovement = true;
    }
}
