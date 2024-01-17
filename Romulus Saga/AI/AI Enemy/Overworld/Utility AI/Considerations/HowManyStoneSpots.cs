using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HowManyStoneSpots", menuName = "UtilityAI/Considerations/HowManyStoneSpots")]
public class HowManyStoneSpots : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = 1 - responseCurve.Evaluate(Mathf.Clamp01(AI_BuildingSpots.Singleton.StonemineSpots.Count / 4f));
        if (AI_BuildingSpots.Singleton.StonemineSpots.Count == 0)
            score = 0;
        return score;
    }
}
