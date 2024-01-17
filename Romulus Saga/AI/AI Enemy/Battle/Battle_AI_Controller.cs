using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using EasyButtons;

public enum UtilityBattleAIState
{
    decide,
    move,
    attack,
    finished
}

public class Battle_AI_Controller : MonoBehaviour
{
    
    // Enemy Unit movement on the hexatiles. With choosing the best decision
    
    
    public Battle_AI_Brain aiBrain { get; set; }
    public UtilityBattleAIState currentState { get; set; }
    
    
    public UnitData unitData;
    public static bool startedMoving;
    public bool confirmMovement;
    private Camera battleCamera;

    [Header("Movement Properties")] 
    [HideInInspector] public Vector3 unitOffset;

    [Header("Unit Properties")]
    public HexaTile unitCurrentTile;
    public HexaTile unitDestinationTile;
    public bool unitSelected = false;
    public bool canAttack;
    [HideInInspector] public List<Battle_AI_Controller> units = new();
    [HideInInspector] public int ID;
    public Battle_AI_Controller thisUnit;

    [Header("Range")]
    public List<HexaTile> movementRange = new();
    public List<HexaTile> attackRange = new();
    public List<UnitCombat> playerInAttackRange = new();
    public List<HexaTile> teammatesRange = new();
    public List<HexaTile> playersInMovementRangeTiles = new();
    public List<HexaTile> playerOnTile = new();

    [Header("Components")]
    [HideInInspector] public Enemy_Pathfinder pathfinder;
    [HideInInspector] public BattleMenuController battleMenuController;
    public Battle_AI_UnitCombat unitCombat;
    [HideInInspector] public Animator anim;

    private bool calledOnce = false;

    [Header("Lists")]
    public Battle_AI_EnemyController orderOfUnits;
    private List<GameObject> listOfPlayerUnits = new List<GameObject>();
    
    private float checkForHighestHealth = 200;
    private UnitCombat playerUnit;

    private void Awake()
    {
        aiBrain = GetComponent<Battle_AI_Brain>();
        currentState = UtilityBattleAIState.decide;
        pathfinder = GetComponent<Enemy_Pathfinder>();
        battleCamera = GameObject.Find("BattleCamera").GetComponent<Camera>();
        battleMenuController = FindObjectOfType<BattleMenuController>();
        unitCombat = GetComponent<Battle_AI_UnitCombat>();
        anim = GetComponent<Animator>();

        for (var i = 0; i < GameObject.FindGameObjectsWithTag("EnemyUnits").Length; i++)
            units.Add(GameObject.FindGameObjectsWithTag("EnemyUnits")[i].GetComponent<Battle_AI_Controller>());
        
        ID = units.IndexOf(this);
    }
    private void Start()
    {
        thisUnit = units[ID];
    }
    
    private void Update()
    {
        if (unitSelected)
            FSMTick();
        
        if(thisUnit.unitDestinationTile != thisUnit.unitCurrentTile && thisUnit.unitDestinationTile != null)
            anim.SetBool("isWalking", true);
        else 
            anim.SetBool("isWalking", false);
    }

    private void FixedUpdate()
    {
        DetectCurrentTile();

        if(thisUnit.unitCurrentTile != null)
            thisUnit.unitOffset = thisUnit.unitCurrentTile.isHigh ? new Vector3(0, .4f, 0) : new Vector3(0, 0.065f, 0);
    }

    void FSMTick()
    {
        if (currentState == UtilityBattleAIState.decide)
        {
            if (!calledOnce)
                thisUnit.ShowMovementRange(unitCombat.currentMovementRange, unitCombat.currentAttackRange);
            aiBrain.DecideBestAction();

            if (unitCurrentTile == unitDestinationTile)
                currentState = UtilityBattleAIState.finished;
            else
                currentState = UtilityBattleAIState.move;
        }
        else if (currentState == UtilityBattleAIState.move)
        {
            if (confirmMovement) // wenn ausgewählte Unit gültiges Ziel hat starte Movement
            {
                startedMoving = true;
                confirmMovement = false;
                StartCoroutine(MoveUnit());
            }

            if (unitCurrentTile == unitDestinationTile)
            {
                thisUnit.ShowMovementRange(0, thisUnit.unitCombat.currentAttackRange);
                currentState = UtilityBattleAIState.finished;
            }

        }
        else if (currentState == UtilityBattleAIState.attack)
        {
            
            if (playerInAttackRange.Count <= 0)
            {
                canAttack = false;
                currentState = UtilityBattleAIState.finished;
                return;
            }

            checkForHighestHealth = 20000;
            for (int i = 0; i < playerInAttackRange.Count; i++)
            {
                float playerHp = playerInAttackRange[i].currentHP * playerInAttackRange[i].numberOfUnits;
                playerHp -= unitCombat.currentDamage * unitCombat.numberOfUnits;
                if (playerHp < checkForHighestHealth)
                {
                    checkForHighestHealth = playerHp;
                    playerUnit = playerInAttackRange[i];
                }
            }
            thisUnit.anim.SetTrigger("isBasicAttacking");
            if(thisUnit.unitData.unitType == UnitData.UnitType.Horseman)
                thisUnit.gameObject.transform.GetChild(0).GetComponent<HorseAnimControllerEnemy>().anim.SetTrigger("isBasicAttacking");
            unitCombat.RemoveHealth(playerUnit, thisUnit.unitCombat);
            canAttack = false;
            
            //aiBrain.DecideBestAttack();
            //canAttack = false;
            currentState = UtilityBattleAIState.finished;
        }
        else if (currentState == UtilityBattleAIState.finished)
        {
            
//            Debug.Log("Controller List: " + playerInAttackRange.Count + " This Name: " + gameObject.name);
            if (playerInAttackRange.Count > 0 && canAttack && unitCurrentTile == unitDestinationTile)
            {
                thisUnit.movementRange.Clear();
                thisUnit.attackRange.Clear();
                thisUnit.playerInAttackRange.Clear();
                thisUnit.teammatesRange.Clear();
                thisUnit.playerOnTile.Clear();
                thisUnit.playersInMovementRangeTiles.Clear();
                thisUnit.ShowMovementRange(unitCombat.currentMovementRange, unitCombat.currentAttackRange);
                currentState = UtilityBattleAIState.attack;
            }
            else
            {
                if (unitCurrentTile == unitDestinationTile)
                {
                    if (unitCurrentTile.isOccupiedByEnemy)
                    {
                        ClearRangeLists();
                        currentState = UtilityBattleAIState.decide;
                        unitSelected = false;
                        calledOnce = false;
                        orderOfUnits.isUnitSelected = false;
                        canAttack = true;
                    }
                    else
                    {
                        ClearRangeLists();
                        currentState = UtilityBattleAIState.decide;
                        unitSelected = false;
                        calledOnce = false;
                        orderOfUnits.isUnitSelected = false;
                        canAttack = true;
                    }
                }
            }
        }
    }

    public void MoveToDestination()
    {
        thisUnit.pathfinder.FindPath(thisUnit.unitCurrentTile, unitDestinationTile);
    }

    #region I did not write everything in this code
    public void GetTiles()
    {
        List<HexaTile> newNeighbours = new List<HexaTile>();
        
        foreach (var neighbour in thisUnit.unitCurrentTile.neighbours)
            if(!thisUnit.movementRange.Contains(neighbour))
                thisUnit.movementRange.Add(neighbour);
        
        for(int i = 0; i < thisUnit.movementRange.Count; i++)
            foreach (var neighbour in thisUnit.movementRange[i].neighbours)
                if (!newNeighbours.Contains(neighbour))
                    newNeighbours.Add(neighbour);

        foreach(var tile in newNeighbours)
            if(!thisUnit.movementRange.Contains(tile))
                thisUnit.movementRange.Add(tile);
        
        GetTeammatesRange();
        GetEnemysRange();
        calledOnce = true;
    }

    
    
    private IEnumerator MoveUnit()
    {
        thisUnit.unitCurrentTile.hexaOutline.GetComponent<SpriteRenderer>().color = HexaTile.green;

        var animationTime = 0f;
        var currentStep = 0;
        var currentPos = thisUnit.unitCurrentTile.transform.position + unitOffset;

        while (currentStep <= thisUnit.pathfinder.pathLength)
        {
            yield return null;
            var nextPos = thisUnit.pathfinder.pathTiles[currentStep].transform.position + unitOffset;
            var movementTime = animationTime / 1.2f;
            
            thisUnit.transform.position = Vector3.MoveTowards(currentPos, nextPos, movementTime);
            animationTime += unitData.movementSpeed * Time.deltaTime;

            if (Vector3.Distance(thisUnit.transform.position, nextPos) >= 0.05f)
                continue;

            currentPos = thisUnit.pathfinder.pathTiles[currentStep].transform.position + unitOffset;
            currentStep++;
            animationTime = 0;
        }
        startedMoving = false;
        thisUnit.unitDestinationTile = null;
        pathfinder.ResetPathfinding();
    }
    
    private void DetectCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(thisUnit.transform.position + new Vector3(0, .65f, 0), transform.TransformDirection(Vector3.down), out hit, 2))
            thisUnit.unitCurrentTile = hit.transform.gameObject.GetComponent<HexaTile>();

        //if (GenerateBattlefield.instance.centerTiles.Contains(thisUnit.unitCurrentTile))
         //   Debug.Log(thisUnit.unitData.unitName.GetLocalizedString() + " ist in der Mitte!");
    }

    private void GetTeammatesRange()
    {
        List<HexaTile> teammatesNeighbours = new List<HexaTile>();
        List<HexaTile> compareNeighbours = new List<HexaTile>();
        List<HexaTile> clearList = new List<HexaTile>();
        List<HexaTile> finalList = new List<HexaTile>();

        foreach (var tile in movementRange)
            if (tile.isOccupiedByEnemy)
                if(!teammatesNeighbours.Contains(tile))
                    teammatesNeighbours.Add(tile);
        
        foreach (var neighbour in teammatesNeighbours)
            compareNeighbours.AddRange(neighbour.neighbours);

        foreach (var containsTile in compareNeighbours)
            if (movementRange.Contains(containsTile))
                clearList.Add(containsTile);

        foreach (var tile in clearList)
            if(!finalList.Contains(tile) && !tile.isOccupied && !tile.isOccupiedByEnemy)
                    finalList.Add(tile);

        foreach (var neighbour in unitCurrentTile.neighbours)
            finalList.Remove(neighbour);
        if (compareNeighbours.Contains(thisUnit.unitCurrentTile)) 
            compareNeighbours.Remove(thisUnit.unitCurrentTile);
        teammatesRange = finalList;
        
    }

    private void GetEnemysRange()
    {
        List<HexaTile> playerUnitsTile = new List<HexaTile>();
        List<HexaTile> playerUnitsTileNeighbour = new List<HexaTile>();
        List<HexaTile> finalList = new List<HexaTile>();
        
        foreach(var tile in movementRange)
            if(tile.isOccupied)
                if(!playerUnitsTile.Contains(tile))
                    playerUnitsTile.Add(tile);
        playerOnTile = playerUnitsTile;
        
        foreach(var neighbour in playerUnitsTile)
            playerUnitsTileNeighbour.AddRange(neighbour.neighbours);
        foreach (var containsTile in playerUnitsTileNeighbour)
            if (movementRange.Contains(containsTile))
                if(!finalList.Contains(containsTile) && !containsTile.isOccupiedByEnemy && !containsTile.isOccupied && containsTile.isWalkable)
                    finalList.Add(containsTile);

        playersInMovementRangeTiles = finalList;
    }

    public void ClearRangeLists()
    {
        thisUnit.movementRange.Clear();
        thisUnit.attackRange.Clear();
        thisUnit.playerInAttackRange.Clear();
        thisUnit.teammatesRange.Clear();
        thisUnit.playerOnTile.Clear();
        thisUnit.playersInMovementRangeTiles.Clear();
    }

    public void ShowMovementRange(int _movementRange, int _attackRange)
    {
        // Zeige die _movementRange an (s. GetMovementRange)
        // Zeige die AttackRange an
        //    -> wenn Range = 1, dann aktiviere nur die Nachbarfelder. GetAttackRange() funktioniert nicht bei Range = 1
        
        for (int i = 0; i < _movementRange - 1; i++)
            Invoke(nameof(GetTiles), 0);

        if (_attackRange == 1)
        {
            foreach (var tile in thisUnit.unitCurrentTile.neighbours)
                if(!thisUnit.attackRange.Contains(tile))
                    thisUnit.attackRange.Add(tile);
            foreach (var tile in attackRange)
            {
               // if (!tile.isOccupied || tile.isOccupiedByEnemy)
                 //   tile.attackFill.SetActive(true);
                if (tile.playerOnTile != null && !playerInAttackRange.Contains(tile.playerOnTile))
                    playerInAttackRange.Add(tile.playerOnTile);
            }
        }
        else
            for (int i = 0; i < _attackRange - 1; i++)
                Invoke(nameof(GetAttackRange), 0);
    }
    #endregion
    
    #region BestScenarios

    [Button]
    public void RemoveOccupiedTiles()
    {
        var playerUnits = FindObjectOfType<UnitController>();
        
        for (int i = 0; i < units.Count; i++)
            thisUnit.movementRange.Remove(units[i].unitCurrentTile);
        
        for (int i = 0; i < playerUnits.units.Count; i++)
            thisUnit.movementRange.Remove(playerUnits.units[i].unitCurrentTile);
    }
    #endregion
    
    public void GetBestCenterTile()
    {
        // Firt movement is random towards the mid, after that, it checks for a center tile
        if (BattleRoundManager.battleRoundCounter == 0)
        { 
            //("Stage 1");   
            List<HexaTile> sortedList = new();
            List<HexaTile> randomDestinationTiles = new();
        
            foreach (var tile in thisUnit.movementRange)
                if(!sortedList.Contains(tile))
                    sortedList.Add(tile);
            
            sortedList.Reverse();

            foreach (var tile in sortedList)
                if(tile.tilePosition.x == sortedList[0].tilePosition.x)
                    randomDestinationTiles.Add(tile);
            
            thisUnit.unitDestinationTile = randomDestinationTiles[Random.Range(0, randomDestinationTiles.Count)];
            
            sortedList.Clear();
            randomDestinationTiles.Clear();
        }
        else if (BattleRoundManager.battleRoundCounter == 1)
        {
            //("Stage 2");
            List<HexaTile> centerTiles = new();

            foreach (var tile in thisUnit.movementRange)
                if(GenerateBattlefield.instance.centerTiles.Contains(tile))
                    centerTiles.Add(tile);

            if(centerTiles.Count > 0)
                thisUnit.unitDestinationTile = centerTiles[Random.Range(0, centerTiles.Count)];
            else
                thisUnit.unitDestinationTile = thisUnit.movementRange[Random.Range(0, thisUnit.movementRange.Count)];
        }
        else
        {
            List<HexaTile> sortedList = new List<HexaTile>();

            foreach(var tile in thisUnit.movementRange)
                if(!sortedList.Contains(tile))
                    sortedList.Add(tile);
            sortedList.Reverse();

            if (unitCurrentTile.tilePosition.x == 6 || unitCurrentTile.tilePosition.x == 7 ||
                unitCurrentTile.tilePosition.x == 8)
            {
                List<HexaTile> middleTiles = new List<HexaTile>();
                
                foreach(var tile in thisUnit.movementRange)
                    if(tile.tilePosition.x is 6 or 7 or 8)
                        if(!middleTiles.Contains(tile) && !tile.isOccupiedByEnemy && !tile.isOccupied)
                            middleTiles.Add(tile);

                thisUnit.unitDestinationTile = middleTiles[Random.Range(0, middleTiles.Count)];
                sortedList.Clear();
                return;
            }
            else if (unitCurrentTile.tilePosition.x > 8)
            {
                List<HexaTile> randomDestinationTile = new List<HexaTile>();
                foreach(var tile in sortedList)
                    if(tile.tilePosition.x == sortedList[0].tilePosition.x)
                        randomDestinationTile.Add(tile);

                thisUnit.unitDestinationTile = randomDestinationTile[Random.Range(0, randomDestinationTile.Count)];
                sortedList.Reverse();
                randomDestinationTile.Clear();
            }
            else if (unitCurrentTile.tilePosition.x < 6)
            {
                List<HexaTile> randomDestinationTile = new List<HexaTile>();
                
                foreach (var tile in sortedList)
                    if(tile.tilePosition.x == sortedList[sortedList.Count].tilePosition.x)
                        randomDestinationTile.Add(tile);
                thisUnit.unitDestinationTile = randomDestinationTile[Random.Range(0, randomDestinationTile.Count)];
                sortedList.Reverse();
                randomDestinationTile.Clear();
            }
        }
    }

    public void MoveToLocations(HexaTile location)
    {
        thisUnit.unitDestinationTile = location;
    }
    
    
    #endregion
    
}



    

