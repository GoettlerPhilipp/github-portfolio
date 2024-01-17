using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyUnitsAlive", menuName = "UtilityAI/Battle_AI/Consideration/EnemyUnitsAlive")]
public class Battle_EnemyUnitsAlive : Battle_AI_Consideration
{
    //Nachschauen wieviele Gegner noch auf der Karte sind
    [SerializeField]private AnimationCurve responseCurve;
    [SerializeField] private bool forTeamOrEnemy; //False: Team / True: Gegner
    //Für Team Bedeutung: Das er check wieviele Einheiten im Gegner Team noch am leben sind, um bei seinen Einheiten zu bleiben
    //Für Gegner Bedeutung: Das er check wieviele Einheiten im Spieler Team noch am leben sind, um auf "jagt" zu gehen
    public override float ScoreConsideration(Battle_AI_Controller aiNpc)
    {
        if (!forTeamOrEnemy)
        {
            float unitsAtm = aiNpc.orderOfUnits.listOfPlayerUnits.Count;
            float startunits = aiNpc.orderOfUnits.playerUnitsStartNum;
            score = 1 - responseCurve.Evaluate(Mathf.Clamp01(unitsAtm / startunits));
            return score;
        }
        else
        {
            float unitsAtm = aiNpc.orderOfUnits.listOfPlayerUnits.Count;
            float startunits = aiNpc.orderOfUnits.playerUnitsStartNum;
            score = responseCurve.Evaluate(Mathf.Clamp01(unitsAtm / startunits));
            return score;
        }
    }
}
