using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsAlive", menuName = "UtilityAI/Battle_AI/Consideration/UnitsAlive")]
public class Battle_UnitsAlive : Battle_AI_Consideration
{
    //[SerializeField] private float juvenileFighterStayWithTeam = 0.5f; //Wie wichtig der Einheit es ist, mit dem Team zu sein.
    //[SerializeField] private float horsemanStayWithTeam = 0.2f;
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        if (BattleRoundManager.battleRoundCounter < 2)
            return score = 0.1f;
        if (aiNpc.orderOfUnits.listOfEnemyUnits.Count <= 1)
            return score = 0;

        if (aiNpc.teammatesRange.Count == 0)
            return score = 0;
        
        //if (aiNpc.unitData.unitType == UnitData.UnitType.JuvenileFighter)
        //    return juvenileFighterStayWithTeam;
        //if (aiNpc.unitData.unitType == UnitData.UnitType.Horseman)
        //    return horsemanStayWithTeam;
        float unitsAtm = aiNpc.orderOfUnits.listOfEnemyUnits.Count;
        float startunits = aiNpc.orderOfUnits.enemyUnitsStartNum;
        score = Mathf.Clamp01(unitsAtm / 3f) - 0.2f;
        return score;

    }
}
