using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle_AttackUnit", menuName = "UtilityAI/Battle_AI/AttackUnit")]
public class Battle_AttackUnit : Battle_AI_Action
{
    public UnitCombat playerUnit;
    private float checkForHighestHealth = 200;
    public override void Execute(Battle_AI_Controller aiNpc)
    {
        aiNpc.aiBrain.attackFinishedExecutingBestAction = true;
    }

    public override void AttackRequiredPlayer(Battle_AI_Controller aiNpc)
    {
        Debug.Log("Attack The Player: " + aiNpc.playerInAttackRange.Count);
        if(aiNpc.playerInAttackRange.Count <= 0)
            return;
        checkForHighestHealth = 20000;
        Debug.Log("Bin ich schon drin");
        for (int i = 0; i < aiNpc.playerInAttackRange.Count; i++)
        {
            float playerHp = aiNpc.playerInAttackRange[i].currentHP * aiNpc.playerInAttackRange[i].numberOfUnits;
            playerHp -= aiNpc.unitCombat.currentDamage * aiNpc.unitCombat.numberOfUnits;
            if (playerHp < checkForHighestHealth)
            {
                checkForHighestHealth = playerHp;
                playerUnit = aiNpc.playerInAttackRange[i];
            }
        }
        aiNpc.unitCombat.RemoveHealth(playerUnit, aiNpc.thisUnit.unitCombat);
        aiNpc.canAttack = false;
    }
}
