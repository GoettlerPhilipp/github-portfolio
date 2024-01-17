using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "WalkToTeam", menuName = "UtilityAI/Battle_AI/WalkToTeam") ]
public class Battle_WalkToTeam : Battle_AI_Action
{
    public override void Execute(Battle_AI_Controller aiNpc)
    {
        aiNpc.thisUnit.aiBrain.finishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination(Battle_AI_Controller aiNpc)
    {
        if (aiNpc.orderOfUnits.listOfEnemyUnits.Count > 1)
        {
            /*if (aiNpc.thisUnit.unitData.unitType == UnitData.UnitType.JuvenileThrower)
            {
                HexaTile choosenTile = aiNpc.unitCurrentTile;
                int checkHighestX = 0;
                for (int i = 0; i < aiNpc.teammatesRange.Count; i++)
                    if (aiNpc.teammatesRange[i].tilePosition.x > checkHighestX)
                    {
                        checkHighestX = aiNpc.teammatesRange[i].tilePosition.x;
                        choosenTile = aiNpc.teammatesRange[i];
                        
                    }
                Debug.Log("Wtf: " + choosenTile);
                aiNpc.MoveToLocations(choosenTile);
            }
            else*/
            int randomNum = Random.Range(0, aiNpc.teammatesRange.Count);
            aiNpc.thisUnit.RemoveOccupiedTiles();
            aiNpc.MoveToLocations(aiNpc.teammatesRange[randomNum]);

            aiNpc.thisUnit.MoveToDestination();
            aiNpc.thisUnit.confirmMovement = true;
        }
    }
}
