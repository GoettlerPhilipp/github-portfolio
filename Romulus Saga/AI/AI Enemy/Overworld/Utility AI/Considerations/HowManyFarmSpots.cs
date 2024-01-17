using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HowManyFarmSpots", menuName = "UtilityAI/Considerations/HowManyFarmSpots")]
public class HowManyFarmSpots : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = 1 - responseCurve.Evaluate(Mathf.Clamp01(AI_BuildingSpots.Singleton.FarmSpots.Count / 4f));
        if (AI_BuildingSpots.Singleton.FarmSpots.Count == 0)
            score = 0;
        return score;
    }
}
