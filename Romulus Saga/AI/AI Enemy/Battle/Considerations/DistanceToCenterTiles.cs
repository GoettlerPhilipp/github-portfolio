using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DistanceToCenterTiles", menuName = "UtilityAI/Battle_AI/Consideration/DistanceToCenter")]
public class DistanceToCenterTiles : Battle_AI_Consideration
{
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        foreach(var tile in GenerateBattlefield.instance.centerTiles)
            if (aiNpc.thisUnit.unitCurrentTile == tile)
                return score = 0;
        float maxDis = Mathf.Infinity;
        float maxDisTile = 10;
        for (int i = 0; i < GenerateBattlefield.instance.centerTiles.Count; i++)
        {
            float distance = Vector3.Distance(aiNpc.thisUnit.unitCurrentTile.transform.position,
                GenerateBattlefield.instance.centerTiles[i].transform.position);
            if (distance < maxDis)
            {
                maxDis = distance;
                
            }

            float distanceTile = aiNpc.thisUnit.unitCurrentTile.tilePosition.x -
                                GenerateBattlefield.instance.centerTiles[i].tilePosition.x;
            if (distanceTile < maxDisTile)
            {
                maxDisTile = distanceTile;
            }
        }

        score = Mathf.Clamp01(maxDisTile / 6f);
        return score;
    }
}
