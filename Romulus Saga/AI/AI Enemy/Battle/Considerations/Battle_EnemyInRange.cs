using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInRange", menuName = "UtilityAI/Battle_AI/Consideration/EnemyInRange")]
public class Battle_EnemyInRange : Battle_AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        float playersAlive = aiNpc.orderOfUnits.listOfPlayerUnits.Count;
        float playersInRange = aiNpc.playerInAttackRange.Count;

        score = 1 - responseCurve.Evaluate(Mathf.Clamp01(playersInRange / playersAlive));
        return score;
    }
}
