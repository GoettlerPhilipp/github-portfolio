using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AILeaderMovement : MonoBehaviour
{
    
    //EnemyLeader script on gameobject to get in the states
    
    //each Level
    private AILeaderState currentState;
    //only Level 2
    private AI_LeaderStateLevel2 currentStateLevel2;
    //only Level 4
    private AI_LeaderStateLevel4 currentStateLevel4;
    public int myNumber;
    //Responsible for everyone again
    private NavMeshAgent agent;
    private Animator anim;
    public Dictionary<UnitData.UnitType, int> UnitsOnMe;
    public int myUnitsCount;

    private bool currentIndex = true;

    public float wallCooldown; 
    public bool readyToAttackWall;

    [Header("UnitsOnSpawn")] 
    [SerializeField] private int archerNum = 10;
    [SerializeField] private int swordsmanNum = 10;
    [SerializeField] private int horseRiderNum = 10;
    
    public GameObject juvenileThrower;
    public GameObject juvenileFighter;
    public GameObject horseman;

    private bool isLoaded;
    
    // Update is called once per frame
    void Update()
    {
        switch (isLoaded)
        {
            case false:
                UnitsOnMe = new Dictionary<UnitData.UnitType, int>()
                {
                    { UnitData.UnitType.JuvenileThrower, archerNum },
                    { UnitData.UnitType.JuvenileFighter, swordsmanNum },
                    { UnitData.UnitType.Horseman, horseRiderNum }
                };
                anim = GetComponent<Animator>();
            
                agent = GetComponent<NavMeshAgent>();
                if (SceneManager.GetSceneByName("Tutorial_02").isLoaded)
                    currentStateLevel2 = new Patrol_Level2(agent, this.gameObject, UnitsOnMe, anim);
                else if (SceneManager.GetSceneByName("Level04").isLoaded)
                {
                    currentStateLevel4 = new Patrol_Level4(agent, this.gameObject, UnitsOnMe, anim);
                }
                else
                    currentState = new Patrol(agent, this.gameObject, UnitsOnMe, anim);
                AI_StorageInventory.instance.myLeaders.Add(gameObject);
                isLoaded = true;
                break;
        }
        if (PauseMenuController.instance.currentGameState == GameState.Paused || TriggerBattle.startedBattle)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        

        if (SceneManager.GetSceneByName("Tutorial_02").isLoaded)
            currentStateLevel2 = currentStateLevel2.Process();
        else if (SceneManager.GetSceneByName("MainMenu").isLoaded)
            currentState = null;
        else if (SceneManager.GetSceneByName("Level04").isLoaded)
            currentStateLevel4 = currentStateLevel4.Process();
        else if(SceneManager.GetSceneByName("Level03").isLoaded ||
                SceneManager.GetSceneByName("Level05").isLoaded ||
                SceneManager.GetSceneByName("Level06").isLoaded)
            currentState = currentState.Process();
        myUnitsCount = UnitsOnMe[UnitData.UnitType.JuvenileThrower] + UnitsOnMe[UnitData.UnitType.JuvenileFighter] +
                       UnitsOnMe[UnitData.UnitType.Horseman];
        StartTimer();
    }

    public void StartTimer()
    {
        if (wallCooldown > 0)
        {
            readyToAttackWall = false;
            wallCooldown -= 1 * Time.deltaTime;
        }
        else if (wallCooldown <= 0)
            readyToAttackWall = true;

    }
}
