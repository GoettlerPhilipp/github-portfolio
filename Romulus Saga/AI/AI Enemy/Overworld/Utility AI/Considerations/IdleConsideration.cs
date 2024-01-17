using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleConsideration", menuName = "UtilityAI/Considerations/IdleConsideration")]
public class IdleConsideration : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01(aiBase.idleOverTime / 100f));
        return score;
    }
    
}
