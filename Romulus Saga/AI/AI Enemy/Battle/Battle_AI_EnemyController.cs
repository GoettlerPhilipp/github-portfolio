using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle_AI_EnemyController : MonoBehaviour
{
    //Controlls the enemy units in order + win and lose condition
    
    public Queue<GameObject> enemyUnits = new Queue<GameObject>();
    public GameObject controlledUnits;
    public GameObject playerLeader;
    public GameObject enemyLeader;
    
    [HideInInspector] public bool getCalledOnce;
    [HideInInspector] public bool isUnitSelected = false;
    [HideInInspector] public bool testOnce = true;
    [HideInInspector] public bool battleStarted;
    [HideInInspector] public bool battleEnded;

    [Header("List of Units on Battlefield")]
    public List<GameObject> listOfEnemyUnits = new List<GameObject>();
    public List<GameObject> listOfPlayerUnits = new List<GameObject>();
    [HideInInspector] public int enemyUnitsStartNum;
    [HideInInspector] public int playerUnitsStartNum;
    
    
    public bool safeStartStateNum; //Wenn ein neuer Kampf startet, muss der bool reseted werden auf false


    private void Awake()
    {
        safeStartStateNum = false;
    }

    private void Update()
    {
        if(battleEnded)
            return;
        if(!TriggerBattle.startedBattle)
            return;
        
        foreach(var gO in listOfEnemyUnits)
            if (gO == null)
                listOfEnemyUnits.Remove(gO);
        foreach (var gO in listOfPlayerUnits)
            if (gO == null)
                listOfPlayerUnits.Remove(gO);
        
        if (BattleRoundManager.instance.currentState == BattleStates.EnemyTurn && !getCalledOnce)
        {
            if (testOnce)
            {
                listOfPlayerUnits.AddRange(GameObject.FindGameObjectsWithTag("Unit"));
                listOfEnemyUnits.AddRange(GameObject.FindGameObjectsWithTag("EnemyUnits"));
                
                for (int i = 0; i < listOfEnemyUnits.Count; i++)
                {
                    if (listOfEnemyUnits[i].layer != 10)
                        listOfEnemyUnits.Remove(listOfEnemyUnits[i]);
                }
                testOnce = false;
            }
            for (int i = 0; i < listOfEnemyUnits.Count; i++)
            {
                enemyUnits.Enqueue(listOfEnemyUnits[i]);
            }

            playerUnitsStartNum = listOfPlayerUnits.Count;
            enemyUnitsStartNum = listOfEnemyUnits.Count;
            battleStarted = true;
            getCalledOnce = true;
        }
        if (BattleRoundManager.instance.currentState == BattleStates.EnemyTurn)
        {
            UseUnitsInOrder();
        }

        if (enemyUnits.Count == 0 && getCalledOnce)
        {
            BattleRoundManager.instance.currentState = BattleStates.PlayerTurn;
            getCalledOnce = false;
        }

        if (battleStarted)
        {
            BattleIsOver();
        }
    }

    public void UseUnitsInOrder()
    {
        if (!isUnitSelected)
        {
            controlledUnits = enemyUnits.Dequeue();
            controlledUnits.GetComponent<Battle_AI_Controller>().orderOfUnits = this;
            controlledUnits.GetComponent<Battle_AI_Controller>().canAttack = true;
            controlledUnits.GetComponent<Battle_AI_Controller>().unitSelected = true;
            isUnitSelected = true;
        }
    }

    public void BattleIsOver()
    {
        //Enemy won
        if (listOfPlayerUnits.Count <= 0)
        {
            battleEnded = true;

            for (int i = 0; i < listOfEnemyUnits.Count; i++)
            {
                switch (listOfEnemyUnits[i].GetComponent<Battle_AI_UnitCombat>().unitData.unitType)
                {
                    case UnitData.UnitType.JuvenileThrower:
                        enemyLeader.GetComponent<AILeaderMovement>().UnitsOnMe[UnitData.UnitType.JuvenileThrower] =
                            listOfEnemyUnits[i].GetComponent<Battle_AI_UnitCombat>().numberOfUnits;
                        break;
                    case UnitData.UnitType.JuvenileFighter:
                        enemyLeader.GetComponent<AILeaderMovement>().UnitsOnMe[UnitData.UnitType.JuvenileFighter] =
                            listOfEnemyUnits[i].GetComponent<Battle_AI_UnitCombat>().numberOfUnits;
                        break;
                    case UnitData.UnitType.Horseman:
                        enemyLeader.GetComponent<AILeaderMovement>().UnitsOnMe[UnitData.UnitType.Horseman] =
                            listOfEnemyUnits[i].GetComponent<Battle_AI_UnitCombat>().numberOfUnits;
                        break;
                }
            }
            
            for(int i = 0; i < listOfEnemyUnits.Count; i++)
                Destroy(listOfEnemyUnits[i]);
            
            enemyLeader.GetComponent<TriggerBattle>().EndBattle(1);
            listOfEnemyUnits.Clear();
        }
        //Player won
        if (listOfEnemyUnits.Count <= 0)
        {
            battleEnded = true;
            
            playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileThrower] = 0;
            playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileFighter] = 0;
            playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.Horseman] = 0;
            
            for (int i = 0; i < listOfPlayerUnits.Count; i++)
            {
                switch (listOfPlayerUnits[i].GetComponent<UnitCombat>().unitData.unitType)
                {
                    case UnitData.UnitType.JuvenileThrower:
                        playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileThrower] +=
                            listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                        break;
                    case UnitData.UnitType.JuvenileFighter:
                        playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileFighter] +=
                            listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                        break;
                    case UnitData.UnitType.Horseman:
                        playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.Horseman] +=
                            listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                        break;
                }
            }
            for (int i = 0; i < listOfPlayerUnits.Count; i++)
                Destroy(listOfPlayerUnits[i]);
            
            enemyLeader.GetComponent<TriggerBattle>().EndBattle(0);
            listOfPlayerUnits.Clear();
        }
    }
}
