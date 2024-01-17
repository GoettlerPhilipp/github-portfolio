using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AI_LeaderStateLevel2
{
    // Only for tutorial level 2, so that the functions are designed to be attacked only after operations.
    public enum STATE{ Patrol_Level2, FollowPlayer_Level2, AttackBase_Level2, PickUnitsUp_Level2, AttackWall_Level2}
    public enum EVENT{ENTER, UPDATE, EXIT}

    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected GameObject npc;
    protected AI_LeaderStateLevel2 nextState;
    protected Dictionary<UnitData.UnitType, int> UnitsOnMe;
    protected Animator anim;

    public GameObject closestPlayer;
    protected GameObject closestWall;

    public int chanceToAttack = 2;

    public AI_LeaderStateLevel2(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
    {
        agent = _agent;
        npc = _npc;
        UnitsOnMe = _unitsOnMe;
        anim = _anim;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() {stage = EVENT.UPDATE;}
    public virtual void Update() {stage = EVENT.UPDATE;}
    public virtual void Exit() {stage = EVENT.EXIT;}

    public AI_LeaderStateLevel2 Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if(stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool PlayerInRange()
    {
        List<GameObject> playerUnits = new List<GameObject>();
        playerUnits.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        for (int i = 0; i < playerUnits.Count; i++)
        {
            float distance = Vector3.Distance(playerUnits[i].transform.position, npc.transform.position);
            if (distance <= 20)
            {
                closestPlayer = playerUnits[i];
                return true;
            }
        }
        return false;
    }

    //Check if player has enough units
    public bool PlayerUnitsInInventory()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerUnitsInventory>();
        int countUnits = player.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileFighter] +
                         player.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.JuvenileThrower] +
                         player.GetComponent<PlayerUnitsInventory>().UnitsOnMe[UnitData.UnitType.Horseman];
        GameObject playerBase = GameObject.FindGameObjectWithTag("PlayerBase");
        int playerBaseCountUnits = 0;
        if (playerBase != null)
        {
            playerBaseCountUnits =
                BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileFighter] +
                BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileThrower] +
                BaseInventory.instance.UnitsInBase[UnitData.UnitType.Horseman];
        }


        if (countUnits >= 5 || playerBaseCountUnits >= 10)
            return true;

        return false;
    }
    //Check if player has a barrack
    public bool PlayerHasBarrack()
    {
        GameObject playerBarrack = GameObject.FindGameObjectWithTag("Barrack");
        if (playerBarrack != null)
            return true;
        return false;
    }
    //Check whether the first battle is completed so that units can be collected immediately
    public bool FirstFightIsOver()
    {
        if (Tutorial_Level2Conditions.instance.FightGotTriggerd)
            return true;
        return false;
    }
    public bool WallInRange()
    {
        List<WallHealthController> walls = new List<WallHealthController>();
        List<WallHealthController> finishedWallList = new List<WallHealthController>();
        walls.AddRange(GameObject.FindObjectsOfType<WallHealthController>());
        foreach (var wall in walls)
            if (!wall.gameObject.CompareTag("EnemyWalls"))
                if(!finishedWallList.Contains(wall))
                    finishedWallList.Add(wall);

        for (int i = 0; i < finishedWallList.Count; i++)
        {
            float distance = Vector3.Distance(npc.transform.position, finishedWallList[i].transform.position);
            if (distance <= 20)
            {
                closestWall = finishedWallList[i].gameObject;
                return true;
            }
        }
        return false;
    }
}

public class Patrol_Level2 : AI_LeaderStateLevel2
{

    private int randomNumberForAttackPlayer;
    private int randomNumberPickUnits;

    private GameObject choosenWaypoint;
    private int randomNumWaypoints;
    
    private Vector3 currentPosForSeconds;
    private float timer = 5f;
    public Patrol_Level2(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.Patrol_Level2;
    }

    public override void Enter()
    {
        randomNumWaypoints = Random.Range(0, WaypointList.instance.waypoints.Count);
        choosenWaypoint = WaypointList.instance.waypoints[randomNumWaypoints];
        if (choosenWaypoint != null)
            agent.SetDestination(choosenWaypoint.transform.position);
        anim.SetBool("isWalking", true);
        if(agent.hasPath)
            base.Enter();
    }

    public override void Update()
    {
        Vector3 currentPos = npc.transform.position;
        if (currentPos != currentPosForSeconds)
            currentPosForSeconds = currentPos;
        else if (currentPos == currentPosForSeconds)
        {
            if (timer > 0)
                timer -= 1 * Time.deltaTime;
            else if (timer <= 0)
            {
                randomNumWaypoints = Random.Range(0, WaypointList.instance.waypoints.Count);
                choosenWaypoint = WaypointList.instance.waypoints[randomNumWaypoints];
                agent.SetDestination(choosenWaypoint.transform.position);
                timer = 5f;
            }
            
        }
        if (Vector3.Distance(npc.transform.position, choosenWaypoint.transform.position) < 2)
        {
            if (FirstFightIsOver())
            {
                randomNumberPickUnits = Random.Range(0, 16);
                if (randomNumberPickUnits == 0 && npc.GetComponent<AILeaderMovement>().myUnitsCount < 60 && GameObject.FindGameObjectWithTag("EnemyBase") != null)
                {
                    nextState = new PickUnitsUp_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                }
            }

            if (GameObject.FindGameObjectWithTag("PlayerBase") != null)
            {
                if (PlayerUnitsInInventory())
                {
                    randomNumberForAttackPlayer = Random.Range(0, chanceToAttack);
                    if (randomNumberForAttackPlayer == 0)
                    {
                        nextState = new AttackPlayerBase_Level2(agent, npc, UnitsOnMe, anim);
                        stage = EVENT.EXIT;
                        chanceToAttack = 36;
                    }
                }
            }

            randomNumWaypoints = Random.Range(0, WaypointList.instance.waypoints.Count);
            choosenWaypoint = WaypointList.instance.waypoints[randomNumWaypoints];
            agent.SetDestination(choosenWaypoint.transform.position);
        }
        if(PlayerInRange())
        {
            nextState = new FollowPlayer_Level2(agent, npc, UnitsOnMe, closestPlayer, anim);
            stage = EVENT.EXIT;
        }
        if(npc.GetComponent<AILeaderMovement>().readyToAttackWall)
            if (WallInRange())
            {
                int index = 0;
                nextState = new AttackWall_Level2(agent, npc, UnitsOnMe, closestWall, index, anim);
                stage = EVENT.EXIT;
            }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class FollowPlayer_Level2 : AI_LeaderStateLevel2
{
    public GameObject player;
    public FollowPlayer_Level2(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, GameObject _player, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.FollowPlayer_Level2;
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(npc.transform.position, player.transform.position) > 30)
        {
            nextState = new Patrol_Level2(agent, npc, UnitsOnMe, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class PickUnitsUp_Level2 : AI_LeaderStateLevel2
{
    private GameObject myBase;
    private Dictionary<UnitData.UnitType, int> myBaseUnits;
    public PickUnitsUp_Level2(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.PickUnitsUp_Level2;
    }

    public override void Enter()
    {
        if (GameObject.FindWithTag("EnemyBase") != null)
        {
            myBase = GameObject.FindGameObjectWithTag("EnemyBase");
            myBaseUnits = myBase.GetComponent<AI_StorageInventory>().UnitsInBase;
            agent.SetDestination(myBase.transform.position);
            base.Enter();
        }
    }

    public override void Update()
    {
        if (Vector3.Distance(npc.transform.position, myBase.transform.position) < 1)
        {
            anim.SetBool("isWalking", false);
            int throwerNum = Random.Range(1, myBaseUnits[UnitData.UnitType.JuvenileThrower] / 2);
            int fighterNum = Random.Range(1, myBaseUnits[UnitData.UnitType.JuvenileFighter] / 2);
            int horsemanNum = Random.Range(1, myBaseUnits[UnitData.UnitType.Horseman] / 2);

            myBaseUnits[UnitData.UnitType.JuvenileThrower] -= throwerNum;
            UnitsOnMe[UnitData.UnitType.JuvenileThrower] += throwerNum;
            myBaseUnits[UnitData.UnitType.JuvenileFighter] -= fighterNum;
            UnitsOnMe[UnitData.UnitType.JuvenileFighter] += fighterNum;
            myBaseUnits[UnitData.UnitType.Horseman] -= horsemanNum;
            UnitsOnMe[UnitData.UnitType.Horseman] += horsemanNum;
            

            nextState = new Patrol_Level2(agent, npc, UnitsOnMe, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class AttackPlayerBase_Level2 : AI_LeaderStateLevel2
{
    private GameObject playerBase;
    private bool attackingBase;
    public AttackPlayerBase_Level2(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.AttackBase_Level2;
    }

    public override void Enter()
    {
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase");
        agent.SetDestination(playerBase.transform.position);
        
        base.Enter();
    }

    public override void Update()
    {
        if(!attackingBase)
            if(npc.GetComponent<AILeaderMovement>().readyToAttackWall)
                if (WallInRange())
                {
                    int index = 1;
                    nextState = new AttackWall_Level2(agent, npc, UnitsOnMe, closestWall, index, anim);
                    stage = EVENT.EXIT;
                }
        if (Vector3.Distance(npc.transform.position, playerBase.transform.position) < 15)
        {
            anim.SetBool("isAttacking", true);
            attackingBase = true;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isAttacking", false);
        base.Exit();
    }
}

public class AttackWall_Level2 : AI_LeaderStateLevel2
{
    private GameObject currentWall;
    private int pastStateIndex;
    private int indexOfWall = 3;
    private bool isAttacking;
    private float attackSpeed = 1.5f;
    private int damageAgainstWall = 1;

    public AttackWall_Level2(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe,
        GameObject _closestWall, int _index, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.AttackWall_Level2;
        currentWall = _closestWall;
        pastStateIndex = _index;
    }

    public override void Enter()
    {
        agent.SetDestination(currentWall.transform.position);
        if (Vector3.Distance(npc.transform.position, currentWall.transform.position) < 2)
        {
            anim.SetBool("isAttacking", true);
            base.Enter();
        }

        if (currentWall == null || Vector3.Distance(currentWall.transform.position, npc.transform.position) > 25)
        {
            switch (pastStateIndex)
            {
                case 0:
                    nextState = new Patrol_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    nextState = new AttackPlayerBase_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
            }
        }
    }

    public override void Update()
    {
        if (indexOfWall > 0)
        {
            DamageWall(currentWall);
            if (currentWall == null)
            {

                indexOfWall--;
                List<WallHealthController> walls = new List<WallHealthController>();
                List<WallHealthController> finishedWallList = new List<WallHealthController>();
                walls.AddRange(GameObject.FindObjectsOfType<WallHealthController>());
                foreach (var wall in walls)
                    if (!wall.gameObject.CompareTag("EnemyWalls"))
                        if (!finishedWallList.Contains(wall))
                            finishedWallList.Add(wall);
                float maxDis = 20f;
                for (int i = 0; i < finishedWallList.Count; i++)
                {
                    float distance = Vector3.Distance(npc.transform.position, finishedWallList[i].transform.position);
                    if (distance <= maxDis)
                    {
                        maxDis = distance;
                        currentWall = finishedWallList[i].gameObject;
                    }
                }
            }
        }
        else if (indexOfWall <= 0)
        {
            switch (pastStateIndex)
            {
                case 0:
                    nextState = new Patrol_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    nextState = new AttackPlayerBase_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
            }

            npc.GetComponent<AILeaderMovement>().wallCooldown = 60f;

        }

        if (Vector3.Distance(currentWall.transform.position, npc.transform.position) > 25)
        {
            switch (pastStateIndex)
            {
                case 0:
                    nextState = new Patrol_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    nextState = new AttackPlayerBase_Level2(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
            }
        }
    }

    public override void Exit()
    {
        anim.SetBool("isAttacking", false);
        base.Exit();
    }

    private void DamageWall(GameObject _wall)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _wall.transform.GetComponent<WallHealthController>().ReceiveDamage(damageAgainstWall);
        }
        else
        {
            if (attackSpeed > 0)
                attackSpeed -= 1 * Time.deltaTime;
            else if (attackSpeed <= 0)
            {
                attackSpeed = 1.5f;
                isAttacking = false;
            }
        }
    }
}

