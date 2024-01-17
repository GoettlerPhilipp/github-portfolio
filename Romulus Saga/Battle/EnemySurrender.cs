using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySurrender : MonoBehaviour
{
    private Battle_AI_EnemyController getAllUnits;

    private int playerUnits = 0;
    private int enemyUnits;

    private bool getCalledOnce;

    [SerializeField] public GameObject surrenderButton;
    // Start is called before the first frame update
    void Start()
    {
        getAllUnits = FindObjectOfType<Battle_AI_EnemyController>().GetComponent<Battle_AI_EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (getAllUnits.battleStarted)
        {
            if (BattleRoundManager.instance.currentState == BattleStates.PlayerTurn && !getCalledOnce)
            {
                getCalledOnce = true;
                playerUnits = 0;
                enemyUnits = 0;
                for (int i = 0; i < getAllUnits.listOfPlayerUnits.Count; i++)
                {
                    playerUnits += getAllUnits.listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                }
                for (int i = 0; i < getAllUnits.listOfEnemyUnits.Count; i++)
                {
                    enemyUnits += getAllUnits.listOfEnemyUnits[i].GetComponent<Battle_AI_UnitCombat>().numberOfUnits;
                }
            }
            else if (BattleRoundManager.instance.currentState == BattleStates.EnemyTurn)
            {
                getCalledOnce = false;
            }
            if (CalculateDifference())
                surrenderButton.SetActive(true);
            else
                surrenderButton.SetActive(false);
        }
    }

    public void EnemySurrenders()
    {
        getAllUnits.battleEnded = true;
        //Foreach funktioniert irgendwie nicht idk warum
        getAllUnits.playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileThrower] = 0;
        getAllUnits.playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileFighter] = 0;
        getAllUnits.playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.Horseman] = 0;

        for (int i = 0; i < getAllUnits.listOfPlayerUnits.Count; i++)
        {
            switch (getAllUnits.listOfPlayerUnits[i].GetComponent<UnitCombat>().unitData.unitType)
            {
                case UnitData.UnitType.JuvenileThrower:
                    getAllUnits.playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileThrower] +=
                        getAllUnits.listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                    break;
                case UnitData.UnitType.JuvenileFighter:
                    getAllUnits.playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileFighter] +=
                        getAllUnits.listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                    break;
                case UnitData.UnitType.Horseman:
                    getAllUnits.playerLeader.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.Horseman] +=
                        getAllUnits.listOfPlayerUnits[i].GetComponent<UnitCombat>().numberOfUnits;
                    break;
            }
        }
            
        for (int i = 0; i < getAllUnits.listOfPlayerUnits.Count; i++)
            Destroy(getAllUnits.listOfPlayerUnits[i]);
        
        for(int i = 0; i < getAllUnits.listOfEnemyUnits.Count; i++)
            Destroy(getAllUnits.listOfEnemyUnits[i]);
            
        getAllUnits.listOfPlayerUnits.Clear();
        getAllUnits.listOfEnemyUnits.Clear();
        getAllUnits.enemyLeader.GetComponent<TriggerBattle>().EndBattle(2);
    }

    bool CalculateDifference()
    {
        float differnce;
        differnce = (float)enemyUnits / playerUnits;
        if (differnce == 0)
            return false;
        if (differnce <= 0.25f)
            return true;
        return false;
    }
}
