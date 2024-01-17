using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Animals_State
{
    public enum STATE {IDLE, PATROL, ATTACK}
    public enum EVENT {ENTER, UPDATE, EXIT}

    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected GameObject npc;
    protected AI_Animals_State nextState;
    protected Dictionary<UnitData.UnitType, int> unitsOnMe;
    protected Animator anim;

    protected GameObject closestPlayer;
    
    public AI_Animals_State(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
    {
        agent = _agent;
        npc = _npc;
        unitsOnMe = _unitsOnMe;
        anim = _anim;
        stage = EVENT.ENTER;
    }
    
    public virtual void Enter() {stage = EVENT.UPDATE;}
    public virtual void Update() {stage = EVENT.UPDATE;}
    public virtual void Exit() {stage = EVENT.EXIT;}

    public AI_Animals_State Process()
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
}

public class PATROL : AI_Animals_State
{
    private int randomNumberForAttackPlayer;
    private int randomNumberPickUnits;

    private GameObject choosenWaypoint;
    private int randomNumWaypoints;
    public PATROL(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.PATROL;
    }

    public override void Enter()
    {
        randomNumWaypoints = Random.Range(0, Animal_WaypointList.instance.animalWaypoints.Count);
        choosenWaypoint = Animal_WaypointList.instance.animalWaypoints[randomNumWaypoints];
        agent.SetDestination(choosenWaypoint.transform.position);
        anim.SetBool("isWalking",true);
        base.Enter();
    }

    public override void Update()
    {
        if (Vector3.Distance(npc.transform.position, choosenWaypoint.transform.position) < 2)
        {
            int randomNum = Random.Range(0, 2);
            if (randomNum == 0)
            {
                nextState = new IDLE(agent, npc, unitsOnMe, anim);
                stage = EVENT.EXIT;
                return;
            }
            else
            {
                randomNumWaypoints = Random.Range(0, Animal_WaypointList.instance.animalWaypoints.Count);
                choosenWaypoint = Animal_WaypointList.instance.animalWaypoints[randomNumWaypoints];
                agent.SetDestination(choosenWaypoint.transform.position);
            }
        }
        if (PlayerInRange())
        {
            nextState = new ATTACK(agent, npc, unitsOnMe, closestPlayer, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isWalking", false);
        base.Exit();
    }
}

public class ATTACK : AI_Animals_State
{
    private GameObject player;
    public ATTACK(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, GameObject _player, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.ATTACK;
        player = _player;
    }

    public override void Enter()
    {
        anim.SetBool("isRunning", true);
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(npc.transform.position, player.transform.position) > 30)
        {
            nextState = new PATROL(agent, npc, unitsOnMe, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isRunning",false);
        base.Exit();
    }
}

public class IDLE : AI_Animals_State
{
    private bool getCalledOnce;
    float timer = 5f;
    public IDLE(NavMeshAgent _agent, GameObject _npc, Dictionary<UnitData.UnitType, int> _unitsOnMe, Animator _anim)
        : base(_agent, _npc, _unitsOnMe, _anim)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (!getCalledOnce)
        {
            getCalledOnce = true;
            int num = Random.Range(0, 11);
            if (num == 0)
            {
                nextState = new PATROL(agent, npc, unitsOnMe, anim);
                stage = EVENT.EXIT;
            }
            else if (num == 1 || num == 2)
            {
                anim.SetBool("isIdle", true);
                new WaitForSecondsRealtime(2f);
                anim.SetBool("isIdle", false);
            }
        }

        if (getCalledOnce)
        {
            timer -= 1 * Time.deltaTime;
            if (timer <= 0)
                getCalledOnce = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
