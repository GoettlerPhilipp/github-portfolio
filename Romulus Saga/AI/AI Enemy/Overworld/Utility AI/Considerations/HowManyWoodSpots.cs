using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HowManyWoodSpots", menuName = "UtilityAI/Considerations/HowManyWoodSpots")]
public class HowManyWoodSpots : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = 1 - responseCurve.Evaluate(Mathf.Clamp01(AI_BuildingSpots.Singleton.LumberjackSpots.Count / 4f));
        if (AI_BuildingSpots.Singleton.LumberjackSpots.Count == 0)
            score = 0;
        return score;
    }
}
