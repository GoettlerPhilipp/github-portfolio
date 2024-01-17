using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkToEnemy", menuName = "UtilityAI/Battle_AI/WalkToEnemy")]
public class Battle_WalkToEnemy : Battle_AI_Action
{
    public override void Execute(Battle_AI_Controller aiNpc)
    {
        aiNpc.thisUnit.aiBrain.finishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination(Battle_AI_Controller aiNpc)
    {

        if (aiNpc.attackRange.Count > 1)
        {
            List<HexaTile> randomNeighbourTile = new List<HexaTile>();
            foreach (var tile in aiNpc.unitCurrentTile.neighbours)
                if(!randomNeighbourTile.Contains(tile) && !tile.isOccupied && tile.isWalkable)
                    randomNeighbourTile.Add(tile);
            int randomNum = Random.Range(0, randomNeighbourTile.Count);
            aiNpc.thisUnit.unitDestinationTile = aiNpc.playersInMovementRangeTiles[randomNum];
            //aiNpc.MoveToLocations(randomNeighbourTile[randomNum]);
        }
        else
        {
            int randomNum = Random.Range(0, aiNpc.playersInMovementRangeTiles.Count);
            aiNpc.thisUnit.RemoveOccupiedTiles();
            aiNpc.thisUnit.unitDestinationTile = aiNpc.playersInMovementRangeTiles[randomNum];
            //aiNpc.MoveToLocations(aiNpc.playersInMovementRangeTiles[randomNum]);
        }
        
        aiNpc.thisUnit.MoveToDestination();
        aiNpc.thisUnit.confirmMovement = true;
    }
}
