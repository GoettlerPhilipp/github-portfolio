using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleStateIndex", menuName = "UtilityAI/Battle_AI/Consideration/BattleStateIndex")]
public class Battle_BattleStateIndex : Battle_AI_Consideration
{
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        if (BattleRoundManager.battleRoundCounter < 2)
            return score = 10;

        return 0;
    }
}
