using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public enum BattleStates{PlaningPhase, PlayerTurn, EnemyTurn, Finished};
public class BattleRoundManager : MonoBehaviour
{
    [Header("Battle Round Manager")]
    public static BattleRoundManager instance;
    public BattleStates currentState;
    public static int battleRoundCounter;
    [SerializeField] private TextMeshProUGUI roundStateText;

    [Header("Components")] 
    public List<UnitController> allUnitsOnField = new();
    private SpellManager spellManager;
    
    private void Awake()
    {
        instance = this;
        currentState = BattleStates.PlayerTurn;
        roundStateText.text = currentState.ToString();

        spellManager = FindObjectOfType<SpellManager>();
    }

    private void Update()
    {
        roundStateText.text = currentState.ToString();
        
        if (!TriggerBattle.startedBattle)
            allUnitsOnField.Clear();
    }
    
    private void ResetUnitActions()
    {
        foreach (var unit in allUnitsOnField)
        {
            unit.unitActionsFinished[0] = false;
            unit.unitActionsFinished[1] = false;
        }    
    }

    public void ChangeTurn()
    {
        switch (currentState)
        {
            case BattleStates.PlaningPhase:
                int random = 0;//Random.Range(0, 1);
                if (random == 0)
                {
                    battleRoundCounter = -1;
                    currentState = BattleStates.PlayerTurn;
                }
                else
                {
                    battleRoundCounter = 0;
                    currentState = BattleStates.EnemyTurn;
                }
                roundStateText.text = currentState.ToString();
                break;
            case BattleStates.PlayerTurn:
                battleRoundCounter++;
                ResetUnitActions();
                currentState = BattleStates.EnemyTurn;
                roundStateText.text = currentState.ToString();
                if (spellManager.kampfSchreiActive)
                    spellManager.kampfSchreiStopped = true;
                break;
        }
    }
}
