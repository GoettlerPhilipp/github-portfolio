using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HowMuchStoneIHave", menuName = "UtilityAI/Considerations/HowMuchStoneIHave")]
public class HowMuchStoneIHave : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01(aiBase.baseInventory.RessourcesInBase[RessourceTypes.stone] / 100f));
        return score;
    }
}
