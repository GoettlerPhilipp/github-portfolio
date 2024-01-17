using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HowMuchWoodIHave", menuName = "UtilityAI/Considerations/HowMuchWoodIHave")]
public class HowMuchWoodIHave : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01(aiBase.baseInventory.RessourcesInBase[RessourceTypes.wood]/100f));
        return score;
    }
}