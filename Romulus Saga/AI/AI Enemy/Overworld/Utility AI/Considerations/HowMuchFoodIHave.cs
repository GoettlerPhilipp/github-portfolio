using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HowMuchFoodIHave", menuName = "UtilityAI/Considerations/HowMuchFoodIHave")]
public class HowMuchFoodIHave : AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01(aiBase.baseInventory.FoodInBase[RessourceTypes.food] / 100f));
        return score;
    }
}
