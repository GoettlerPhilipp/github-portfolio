using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIWagonStates
{
    //Different phases the wagon has
    public enum STATE{UNLOADWAGON, CHARGE, WALKSTORAGE, WALKBASE}
    
    public enum EVENT{ENTER, UPDATE, EXIT}
    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected GameObject npc;
    protected AIWagonStates nextState;
    protected Dictionary<RessourceTypes, int> RessourcesInWagon;


    public AIWagonStates(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, int> _ressourcesInWagon)
    {
        agent = _agent;
        anim = _anim;
        npc = _npc;
        stage = EVENT.ENTER;
        RessourcesInWagon = _ressourcesInWagon;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public AIWagonStates Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
    
}

public class UNLOADWAGON : AIWagonStates
{
    private Dictionary<RessourceTypes, int> parentWood;

    private GameObject playerBase;
    
    //For unloading ressources
    
    public UNLOADWAGON(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, int> _ressourcesInWagon)
        : base(_agent, _anim, _npc, _ressourcesInWagon)
    {
        name = STATE.UNLOADWAGON;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        if (npc.layer == 11)
        {
            playerBase = GameObject.FindWithTag("PlayerBase").transform.parent.gameObject;
            parentWood = playerBase.GetComponent<BaseInventory>().RessourcesInInventory;
        }
        else if (npc.layer == 12)
        {
            GameObject enemyBase = GameObject.FindWithTag("EnemyBase");
            parentWood = enemyBase.GetComponent<AI_StorageInventory>().RessourcesInBase;
        }
        
        foreach (KeyValuePair<RessourceTypes, int> inWagon in RessourcesInWagon)
        {
            parentWood[inWagon.Key] += inWagon.Value;
        }
        foreach (KeyValuePair<RessourceTypes, int> inStorage in parentWood)
        {
            RessourcesInWagon[inStorage.Key] = 0 ;
        }
        base.Enter();
    }

    public override void Update()
    {
        nextState = new WALKSTORAGE(agent, anim, npc, RessourcesInWagon);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class WALKSTORAGE : AIWagonStates
{
    //Walking back to his parent object
    public WALKSTORAGE(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, int> _ressourcesInWagon)
        : base(_agent, _anim, _npc, _ressourcesInWagon)
    {
        name = STATE.WALKSTORAGE;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        agent.SetDestination(npc.transform.parent.position);
        base.Enter();
    }

    public override void Update()
    {
        Vector3 distancePos = new Vector3(npc.transform.position.x, 0,npc.transform.position.z);
        Vector3 parentPos = new Vector3(npc.transform.parent.position.x, 0, npc.transform.parent.position.z);
        float distance = Vector3.Distance(distancePos, parentPos);
        if (distance < 1f)
        {
            nextState = new CHARGE(agent, anim, npc, RessourcesInWagon);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class CHARGE : AIWagonStates
{
    private Dictionary<RessourceTypes, int> parentWood;
    //fill up ressources
    public CHARGE(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, int> _ressourcesInWagon)
        : base(_agent, _anim, _npc, _ressourcesInWagon)
    {
        name = STATE.CHARGE;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        parentWood = npc.transform.parent.GetComponent<StorageInventory>().Ressource;

        if (agent.remainingDistance < 1f)
        {
            switch (npc.transform.parent.parent.tag)
            {
                case "Lumberjack":
                    if (parentWood[RessourceTypes.wood] > 0)
                    {
                        if (parentWood[RessourceTypes.wood] <= npc.GetComponent<AIWagonMovement>().maxRessources)
                        {
                            RessourcesInWagon[RessourceTypes.wood] += parentWood[RessourceTypes.wood];
                            parentWood[RessourceTypes.wood] -= RessourcesInWagon[RessourceTypes.wood];
                        }
                        else
                        {
                            RessourcesInWagon[RessourceTypes.wood] = npc.GetComponent<AIWagonMovement>().maxRessources;
                            parentWood[RessourceTypes.wood] -= npc.GetComponent<AIWagonMovement>().maxRessources;
                        }
                        base.Enter();
                    }
                    break;
                case "Stonemine":
                    if (parentWood[RessourceTypes.stone] > 0)
                    {
                        if (parentWood[RessourceTypes.stone] <= npc.GetComponent<AIWagonMovement>().maxRessources)
                        {
                            RessourcesInWagon[RessourceTypes.stone] += parentWood[RessourceTypes.stone];
                            parentWood[RessourceTypes.stone] -= RessourcesInWagon[RessourceTypes.stone];
                        }
                        else
                        {
                            RessourcesInWagon[RessourceTypes.stone] = npc.GetComponent<AIWagonMovement>().maxRessources;
                            parentWood[RessourceTypes.stone] -= npc.GetComponent<AIWagonMovement>().maxRessources;
                        }
                        base.Enter();
                    }
                    break;
                default:
                    Debug.Log("Warum bin ich hier");
                    break;
            }
        }
    }

    public override void Update()
    {
        nextState = new WALKBASE(agent, anim, npc, RessourcesInWagon);
        stage = EVENT.EXIT;
        npc.GetComponent<AIWagonMovement>().parentAndIAreEmpty = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class WALKBASE : AIWagonStates
{
    //walking to the base
    private GameObject Base;
    public WALKBASE(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, int> _ressourcesInWagon)
        : base(_agent, _anim, _npc, _ressourcesInWagon)
    {
        name = STATE.WALKBASE;
        agent.isStopped = false;
        
    }

    public override void Enter()
    {
        if(npc.layer == 11)
            Base = GameObject.FindWithTag("PlayerBase");
        else if(npc.layer == 12)
            Base = GameObject.FindWithTag("EnemyBase");
        agent.SetDestination(Base.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        Vector3 npcPos = new Vector3(npc.transform.position.x, 0,npc.transform.position.z);
        Vector3 basePos = new Vector3(Base.transform.position.x, 0, Base.transform.position.z);
        float distance = Vector3.Distance(npcPos, basePos);
        if (distance < 2f)
        {
            nextState = new UNLOADWAGON(agent, anim, npc, RessourcesInWagon);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

