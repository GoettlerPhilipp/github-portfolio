using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyHealth", menuName = "UtilityAI/Battle_AI/Consideration/MyHealth")]
public class Battle_MyHealth : Battle_AI_Consideration
{
    
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        score = 1 - responseCurve.Evaluate(Mathf.Clamp01(aiNpc.unitCombat.currentHP / aiNpc.unitCombat.maxHP));
        return score;
    }
}
