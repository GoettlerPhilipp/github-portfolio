using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class AI_StorageInventory : MonoBehaviour
{
    //Inventory of the enemy base
    public static AI_StorageInventory instance;

    [SerializeField] private int woodInBase;
    [SerializeField] private int stoneInBase;
    [SerializeField] private int foodInBase;

    public Dictionary<RessourceTypes, int> RessourcesInBase { get; protected set; }
    public Dictionary<RessourceTypes, float> FoodInBase { get; protected set; }

    [Header("Einheiten")] 
    [SerializeField] private int throwerInBase;
    [SerializeField] private int fighterInBase;
    [SerializeField] private int horsemanInBase;

    [Header("Runden Einheit hinzufügen")] 
    [SerializeField] private int addUnitToInventory;
    public Dictionary<UnitData.UnitType, int> UnitsInBase { get; protected set; }

    public List<GameObject> myLeaders = new List<GameObject>();
    public List<GameObject> myBarracks = new List<GameObject>();

    private bool getCalledOnce;
    private bool leaderGetCalledOnce;

    public GameObject leaderPrefab;
    public GameObject enemyListInInspector;

    public virtual void InitializeInventory()
    {
        RessourcesInBase = new Dictionary<RessourceTypes, int>()
        {
            { RessourceTypes.wood, woodInBase },
            { RessourceTypes.stone, stoneInBase },
        };
        FoodInBase = new Dictionary<RessourceTypes, float>()
        {
            { RessourceTypes.food, foodInBase }
        };

        UnitsInBase = new Dictionary<UnitData.UnitType, int>()
        {
            { UnitData.UnitType.JuvenileThrower, throwerInBase },
            { UnitData.UnitType.JuvenileFighter, fighterInBase },
            { UnitData.UnitType.Horseman, horsemanInBase }
        };
    }

    private void Awake()
    {
        instance = this;
        InitializeInventory();
        enemyListInInspector = GameObject.Find("EnemyList");
    }
    // Update is called once per frame
    void Update()
    {
        if(PauseMenuController.instance.currentGameState == GameState.Paused)
            return;

        foreach (var gO in myLeaders)
            if (gO == null)
                myLeaders.Remove(gO);
        foreach (var barrack in myBarracks)
            if(!myBarracks.Contains(barrack))
                myBarracks.Add(barrack);
        
        
        if (!getCalledOnce)
        {
            getCalledOnce = true;
            //AddUnitsToInventory();
            StartCoroutine(AddUnitsToInventoryOverTime());
        }

        if (!leaderGetCalledOnce && !SceneManager.GetSceneByName("Level04").isLoaded)
        {
            leaderGetCalledOnce = true;
            StartCoroutine(AddNewLeader());
        }
    }

    //we didn't have enough time to implement an action and condition, so we worked with random values here.
    #region AddUnits


    IEnumerator AddUnitsToInventoryOverTime()
    {
        yield return new WaitForSecondsRealtime(30f);
        int pickRandomUnit = Random.Range(0,3);
        
        switch (pickRandomUnit)
        {
            case 0:
                UnitsInBase[UnitData.UnitType.JuvenileThrower] += addUnitToInventory + myBarracks.Count -1 ;
                break;
            case 1:
                UnitsInBase[UnitData.UnitType.JuvenileFighter] += addUnitToInventory + myBarracks.Count -1;
                break;
            case 2:
                UnitsInBase[UnitData.UnitType.Horseman] += addUnitToInventory + myBarracks.Count -1;
                break;
        }

        yield return new WaitForSecondsRealtime(5f);
        getCalledOnce = false;
    }

    IEnumerator AddNewLeader()
    {
        yield return new WaitForSecondsRealtime(30f);
        if (myLeaders.Count == 0)
        {
            GameObject newEnemy = Instantiate(leaderPrefab, gameObject.transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(enemyListInInspector.transform);
        }
        else if (myLeaders.Count <= 3)
        {
            int randomNum = Random.Range(0, 31);

            if (randomNum == 0)
            {
                GameObject newEnemy = Instantiate(leaderPrefab, gameObject.transform.position, Quaternion.identity);
                newEnemy.transform.SetParent(enemyListInInspector.transform);
            }
        }
        leaderGetCalledOnce = false;
    }
    void AddUnitsToInventory()
    {
        int pickRandomUnit = Random.Range(0,3);
        
        switch (pickRandomUnit)
        {
            case 0:
                UnitsInBase[UnitData.UnitType.JuvenileThrower] += addUnitToInventory;
                break;
            case 1:
                UnitsInBase[UnitData.UnitType.JuvenileFighter] += addUnitToInventory;
                break;
            case 2:
                UnitsInBase[UnitData.UnitType.Horseman] += addUnitToInventory;
                break;
        }
    }

    #endregion
}
