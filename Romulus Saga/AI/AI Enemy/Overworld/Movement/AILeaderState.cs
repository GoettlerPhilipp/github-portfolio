using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class AILeaderState 
{
    //Enemy Leader state script
    public enum STATE{ Patrol, FollowPlayer, AttackBase, PickUnitsUp, AttackWall}
    public enum EVENT{ENTER, UPDATE, EXIT}

    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected GameObject npc;
    protected AILeaderState nextState;
    protected Dictionary<UnitData.UnitType, int> UnitsOnMe;
    protected Animator anim;

    protected GameObject closestPlayer;
    protected GameObject closestWall;

    public AILeaderState(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
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

    public AILeaderState Process()
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
            if (distance <= 25)
            {
                closestPlayer = playerUnits[i];
                return true;
            }
        }

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

public class Patrol : AILeaderState
{
    private int randomNumberForAttackPlayer;
    private int randomNumberPickUnits;

    private GameObject choosenWaypoint;
    private int randomNumWaypoints;

    private Vector3 currentPosForSeconds;
    private float timer = 5f;
    private int index = 5;
    public Patrol(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.Patrol;
    }

    public override void Enter()
    {
        randomNumWaypoints = Random.Range(0, WaypointList.instance.waypoints.Count);
        choosenWaypoint = WaypointList.instance.waypoints[randomNumWaypoints];
        anim.SetBool("isWalking", true);
        agent.SetDestination(choosenWaypoint.transform.position);
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
            randomNumberPickUnits = Random.Range(0, 16);
            if (randomNumberPickUnits == 0 && index <= 5 && GameObject.FindWithTag("EnemyBase") != null)
            {
                nextState = new PickUnitsUp(agent, npc, UnitsOnMe, anim);
                stage = EVENT.EXIT;
            }
            
            randomNumberForAttackPlayer = Random.Range(0, 36);
            if (randomNumberForAttackPlayer == 0 && index <= 5 && GameObject.FindWithTag("PlayerBase") != null)
            {
                nextState = new AttackPlayerBase(agent, npc, UnitsOnMe, anim);
                stage = EVENT.EXIT;
            }
            
            randomNumWaypoints = Random.Range(0, WaypointList.instance.waypoints.Count);
            choosenWaypoint = WaypointList.instance.waypoints[randomNumWaypoints];
            agent.SetDestination(choosenWaypoint.transform.position);
            index -= 1;
        }
        if (PlayerInRange())
        {
            nextState = new FollowPlayer(agent, npc, UnitsOnMe, closestPlayer, anim);
            stage = EVENT.EXIT;
        }

        if(npc.GetComponent<AILeaderMovement>().readyToAttackWall)
            if (WallInRange())
            {
                int index = 0;
                nextState = new AttackWall(agent, npc, UnitsOnMe, closestWall, index, anim);
                stage = EVENT.EXIT;
            }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class FollowPlayer : AILeaderState
{
    private GameObject player;
    public FollowPlayer(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, GameObject _player, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.FollowPlayer;
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
            nextState = new Patrol(agent, npc, UnitsOnMe, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class PickUnitsUp : AILeaderState
{
    private GameObject myBase;
    private Dictionary<UnitData.UnitType, int> myBaseUnits;
    public PickUnitsUp(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.PickUnitsUp;
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
            

            nextState = new Patrol(agent, npc, UnitsOnMe, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class AttackPlayerBase : AILeaderState
{
    private GameObject playerBase;
    private bool attackingBase;
    public AttackPlayerBase(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.AttackBase;
        attackingBase = false;
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
                    nextState = new AttackWall(agent, npc, UnitsOnMe, closestWall, index, anim);
                    stage = EVENT.EXIT;
                }
        if (Vector3.Distance(npc.transform.position, playerBase.transform.position) < 15)
        {
            anim.SetBool("isWalking", false);
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

public class AttackWall : AILeaderState
{
    private GameObject currentWall;
    private int pastStateIndex;
    private int indexOfWall = 3;
    private bool isAttacking;
    private float attackSpeed = 1.5f;
    private int damageAgainstWall = 1;
    public AttackWall(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, GameObject _closestWall, int _index, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.AttackWall;
        currentWall = _closestWall;
        pastStateIndex = _index;
    }

    public override void Enter()
    {
        agent.SetDestination(currentWall.transform.position);
        if (Vector3.Distance(npc.transform.position, currentWall.transform.position) < 2)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);
            base.Enter();
        }

        if(currentWall == null || Vector3.Distance(currentWall.transform.position, npc.transform.position) > 25)
        {
            switch (pastStateIndex)
            {
                case 0:
                    nextState = new Patrol(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    nextState = new AttackPlayerBase(agent, npc, UnitsOnMe, anim);
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
                        if(!finishedWallList.Contains(wall))
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
                    nextState = new Patrol(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    nextState = new AttackPlayerBase(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
            }
            npc.GetComponent<AILeaderMovement>().wallCooldown = 60f;
            
        }
        
        if(Vector3.Distance(currentWall.transform.position, npc.transform.position) > 25)
        {
            switch (pastStateIndex)
            {
                case 0:
                    nextState = new Patrol(agent, npc, UnitsOnMe, anim);
                    stage = EVENT.EXIT;
                    break;
                case 1:
                    nextState = new AttackPlayerBase(agent, npc, UnitsOnMe, anim);
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
