using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildUnits : MonoBehaviour
{
    //safes PickUnitsUIScript logs in as a referent and holds the queue of individual barracks.
    public static BuildUnits Instance { get; set; }
    [SerializeField] private GetChoosenBarrack PickUnitsUIScript;
    private CurrentUnitsInQueue unitsInQueueScript;
    private bool unitInQueue;
    public BuildUnits thisBarrack;
    private GameObject spawnLeaderInThisScene;
    
    public struct UnitTypeRoundsCount
    {
        public GameObject unit;
        public float timer;
        public Dictionary<RessourceTypes, int> ressourceCosts;
        public int amountAdded;
        public Sprite image;
    }

    //Unit types
    public UnitTypeRoundsCount juvenileFighterUnit;
    public UnitTypeRoundsCount juvenileThrowerUnit;
    public UnitTypeRoundsCount horsemanUnit;
    public UnitTypeRoundsCount playerLeader;

    //Where units spawn/saved after they are finished
    private GameObject playerBase;

    
    //Queue and current unit in training
    public Queue<UnitTypeRoundsCount> queueOfUnits = new Queue<UnitTypeRoundsCount>();
    public UnitTypeRoundsCount nextUnitInProgress = new UnitTypeRoundsCount() { unit = null , timer = 0};

    private bool getCalledOnce = false;

    private void Awake()
    {
        Instance = this;
        PickUnitsUIScript = FindObjectOfType<GetChoosenBarrack>();
        unitsInQueueScript = FindObjectOfType<CurrentUnitsInQueue>();
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase");
        thisBarrack = this;
        PickUnitsUIScript.choosenBarrackScript = this;
        spawnLeaderInThisScene = FindObjectOfType<PlayerSelectionBox>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextUnitInProgress.unit = null;
        thisBarrack.UnitCosts();
    }

    private void Update()
    {
        BuildUnitsTimer();
        FinishedBuildingUnit();
        if (queueOfUnits.Count >= 1 && nextUnitInProgress.timer <= 0)
        {
            nextUnitInProgress = queueOfUnits.Dequeue();
            unitsInQueueScript.duration = nextUnitInProgress.timer;
            unitInQueue = true;
        }
    }
    
    public void SetButtonsToThis()
    {
        unitsInQueueScript.currentBuilding = this;
    }
    public void AddUnitToQueue(UnitTypeRoundsCount unit)
    {
        queueOfUnits.Enqueue(unit);
    }

    private void BuildUnitsTimer()
    {
        if (nextUnitInProgress.timer > 0)
        {
            nextUnitInProgress.timer -= Time.deltaTime;
        }
    }

    private void FinishedBuildingUnit()
    {
        if (nextUnitInProgress.timer <= 0 && unitInQueue)
        {
            if (nextUnitInProgress.unit == juvenileThrowerUnit.unit)
                BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileThrower] +=
                    juvenileThrowerUnit.amountAdded;
            else if (nextUnitInProgress.unit == juvenileFighterUnit.unit)
                BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileFighter] +=
                    juvenileFighterUnit.amountAdded;
            else if (nextUnitInProgress.unit == horsemanUnit.unit)
                BaseInventory.instance.UnitsInBase[UnitData.UnitType.Horseman] += horsemanUnit.amountAdded;
            else if (nextUnitInProgress.unit == playerLeader.unit)
            {
                GameObject newLeader = Instantiate(PickUnitsUIScript.playerLeader, playerBase.transform.position, Quaternion.identity);
                newLeader.transform.parent = spawnLeaderInThisScene.transform;
            }

            nextUnitInProgress.unit = null;
            unitInQueue = false;
        }
    }

    void UnitCosts()
    {
        thisBarrack.juvenileFighterUnit = new UnitTypeRoundsCount()
        {
            unit = PickUnitsUIScript.juvenileFighter,
            timer = 10f,
            image = PickUnitsUIScript.juvenileFighterImg,
            ressourceCosts = new Dictionary<RessourceTypes, int>()
            {
                { RessourceTypes.wood, 0 },
                { RessourceTypes.stone, 5 },
                { RessourceTypes.food, 0 },
                { RessourceTypes.gold, 15 },
                { RessourceTypes.faith, 0 },
                { RessourceTypes.currentResidence, 1 }
            },
            amountAdded = 1,
        };
        thisBarrack.juvenileThrowerUnit = new UnitTypeRoundsCount()
        {
            unit = PickUnitsUIScript.juvenileThrower,
            timer = 10f,
            image = PickUnitsUIScript.juvenileThrowerImg,
            ressourceCosts = new Dictionary<RessourceTypes, int>()
            {
                { RessourceTypes.wood, 15 },
                { RessourceTypes.stone, 0 },
                { RessourceTypes.food, 0 },
                { RessourceTypes.gold, 15 },
                { RessourceTypes.faith, 0 },
                { RessourceTypes.currentResidence, 1 }
            },
            amountAdded = 1
        };
        thisBarrack.horsemanUnit = new UnitTypeRoundsCount()
        {
            unit = PickUnitsUIScript.horseman,
            timer = 20f,
            image = PickUnitsUIScript.horsemanImg,
            ressourceCosts = new Dictionary<RessourceTypes, int>()
            {
                { RessourceTypes.wood, 0 },
                { RessourceTypes.stone, 0 },
                { RessourceTypes.food, 10 },
                { RessourceTypes.gold, 15 },
                { RessourceTypes.faith, 0 },
                { RessourceTypes.currentResidence, 1 }
            },
            amountAdded = 1
        };
        thisBarrack.playerLeader = new UnitTypeRoundsCount()
        {
            unit = PickUnitsUIScript.playerLeader,
            timer = 60f,
            image = PickUnitsUIScript.leaderImg,
            ressourceCosts = new Dictionary<RessourceTypes, int>()
            {
                { RessourceTypes.wood, 5 },
                { RessourceTypes.stone, 5 },
                { RessourceTypes.food, 0 },
                { RessourceTypes.gold, 10 },
                { RessourceTypes.faith, 0 },
                { RessourceTypes.currentResidence, 0 }
            },
            amountAdded = 1,
        };
    }

}
