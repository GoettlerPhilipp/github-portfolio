using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HowManyRessourcesIGot", menuName = "UtilityAI/Considerations/HowManyRessourcesIGot")]
public class HowManyRessourcesIGot : AI_Consideration
{

    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AI_Controller aiBase)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01((aiBase.baseInventory.RessourcesInBase[RessourceTypes.wood] +
                                                        aiBase.baseInventory.RessourcesInBase[RessourceTypes.stone] +
                                                        aiBase.baseInventory.FoodInBase[RessourceTypes.food]) / 100f));
        return score;
    }
}
