using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWagonStateFarmer
{
    //Verschiedenen Phasen, die der Transporteur hat
    public enum STATE{UNLOADWAGON_Farmer, CHARGE_Farmer, WALKSTORAGE_Farmer, WALKBASE_Farmer}
    
    //An welcher Stelle der Phase er sich befindet.
    public enum EVENT{ENTER, UPDATE, EXIT}

    //Nötigen Anforderungen die die States benötigen.
    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected GameObject npc;
    protected AIWagonStateFarmer nextState;
    protected Dictionary<RessourceTypes, float> foodInWagon;
    
    
    public AIWagonStateFarmer(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, float> _foodInWagon)
    {
        agent = _agent;
        anim = _anim;
        npc = _npc;
        stage = EVENT.ENTER;
        foodInWagon = _foodInWagon;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public AIWagonStateFarmer Process()
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

public class UNLOADWAGON_Farmer : AIWagonStateFarmer
{
    private Dictionary<RessourceTypes, float> parentRessources;
    private Dictionary<RessourceTypes, float> enemyParentRessource;

    private GameObject playerBase;
    //STATE ist für das Abladen von Holz.
    public UNLOADWAGON_Farmer(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, float> _foodInWagon)
        : base(_agent, _anim, _npc, _foodInWagon)
    {
        name = STATE.UNLOADWAGON_Farmer;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        if (npc.layer == 11)
        {
            playerBase = GameObject.FindWithTag("PlayerBase").transform.parent.gameObject;
            parentRessources = playerBase.GetComponent<BaseInventory>().HumanRessources;
            parentRessources[RessourceTypes.food] += foodInWagon[RessourceTypes.food];
            foodInWagon[RessourceTypes.food] = 0;
        }
        else if (npc.layer == 12)
        {
            GameObject enemyBase = GameObject.FindWithTag("EnemyBase");
            enemyParentRessource = enemyBase.GetComponent<AI_StorageInventory>().FoodInBase;
            enemyParentRessource[RessourceTypes.food] += (int)foodInWagon[RessourceTypes.food];
            foodInWagon[RessourceTypes.food] = 0;
        }
        base.Enter();
    }

    public override void Update()
    {
        nextState = new WALKSTORAGE_Farmer(agent, anim, npc, foodInWagon);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        //animation
        base.Exit();
    }
}

public class WALKSTORAGE_Farmer : AIWagonStateFarmer
{
    //STATE ist nur dafür, dass der NPC zu seinem ParentObject zurück läuft.
    public WALKSTORAGE_Farmer(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, float> _foodInWagon)
        : base(_agent, _anim, _npc, _foodInWagon)
    {
        name = STATE.WALKSTORAGE_Farmer;
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
            nextState = new CHARGE_Farmer(agent, anim, npc, foodInWagon);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger
        base.Exit();
    }
}

public class CHARGE_Farmer : AIWagonStateFarmer
{
    private Dictionary<RessourceTypes, float> foodInParent;
    //STATE ist zum auffüllen des Wagons da. Er entzieht alle Ressourcen vom Parent Objekt und nimmt es selber auf.
    public CHARGE_Farmer(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, float> _foodInWagon)
        : base(_agent, _anim, _npc, _foodInWagon)
    {
        name = STATE.CHARGE_Farmer;
        agent.isStopped = true;
    }

    public override void Enter()
    {
        foodInParent = npc.transform.parent.GetComponent<StorageInventory>().food;
        if (agent.remainingDistance < 1f)
        {
            if (foodInParent[RessourceTypes.food] > 0)
            {
                if (foodInParent[RessourceTypes.food] <= npc.GetComponent<AIWagonMovementFarmer>().maxRessources)
                {
                    foodInWagon[RessourceTypes.food] += foodInParent[RessourceTypes.food];
                    foodInParent[RessourceTypes.food] -= foodInWagon[RessourceTypes.food];
                }
                else
                {
                    foodInWagon[RessourceTypes.food] = npc.GetComponent<AIWagonMovementFarmer>().maxRessources;
                    foodInParent[RessourceTypes.food] -= npc.GetComponent<AIWagonMovementFarmer>().maxRessources;
                }
                base.Enter();
            }
        }
    }
    public override void Update()
    {
        nextState = new WALKBASE_Farmer(agent, anim, npc, foodInWagon);
        stage = EVENT.EXIT;
        npc.GetComponent<AIWagonMovement>().parentAndIAreEmpty = false;
    }

    public override void Exit()
    {
        //animation
        base.Exit();
    }
}

public class WALKBASE_Farmer : AIWagonStateFarmer
{
    //STATE ist nur dafür da, dass der NPC zu der nächst gelegensten Basis läuft.
    private GameObject Base;
    public WALKBASE_Farmer(NavMeshAgent _agent, Animator _anim, GameObject _npc, Dictionary<RessourceTypes, float> _foodInWagon)
        : base(_agent, _anim, _npc, _foodInWagon)
    {
        name = STATE.WALKBASE_Farmer;
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
            nextState = new UNLOADWAGON_Farmer(agent, anim, npc, foodInWagon);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //Debug.Log("Walkbase End");
        //Debug.Log(nextState);
        base.Exit();
    }
}