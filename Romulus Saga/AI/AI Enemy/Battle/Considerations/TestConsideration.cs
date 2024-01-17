using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Test", menuName = "UtilityAI/Battle_AI/Test")]
public class TestConsideration : Battle_AI_Consideration
{
    public float test;
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        if (BattleRoundManager.battleRoundCounter == 0)
            test = 1;
        else if (BattleRoundManager.battleRoundCounter >= 2)
            test = 0;
        return test;
    }
}
