using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageIDo", menuName = "UtilityAI/Battle_AI/Consideration/DamageIDo")]
public class Battle_DamageIDo : Battle_AI_Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        float dmgScore = 10000f;
        for (int i = 0; i < aiNpc.playerOnTile.Count; i++)
        {
            float enemyHp = aiNpc.playerOnTile[i].playerOnTile.numberOfUnits * aiNpc.playerOnTile[i].playerOnTile.currentHP;
            enemyHp -= aiNpc.unitCombat.currentDamage * aiNpc.unitCombat.numberOfUnits;

            if (enemyHp <= 0)
                return score = 100000;
            if (enemyHp < dmgScore)
            {
                dmgScore = enemyHp;
                score = responseCurve.Evaluate(Mathf.Clamp01(dmgScore / enemyHp));
                Debug.Log("Result: " + score);
                return score;
            }
        }
        
        return score = 0;
    }
}
