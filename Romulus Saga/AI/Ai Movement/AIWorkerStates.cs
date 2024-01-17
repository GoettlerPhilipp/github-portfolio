using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AIWorkerStates
{
    //Verschiedene Phasen, die der Arbeiter hat
    public enum STATE
    {
        WALK, UNLOAD, WORK, WALKHOME,
    }
    //An welcher Stelle der Phase er sich befindet.
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }
    //Nötigen Anforderungen die die States braucht.
    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected GameObject npc;
    protected AIWorkerStates nextState;

    public AIWorkerStates(NavMeshAgent _agent, Animator _anim, GameObject _npc)
    {
        agent = _agent;
        anim = _anim;
        npc = _npc;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() {stage = EVENT.UPDATE;}
    public virtual void Update() {stage = EVENT.UPDATE;}
    public virtual void Exit() {stage = EVENT.EXIT;}

    public AIWorkerStates Process()
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

    public GameObject[] GetNearby(Vector3 origin, float radius)
    {
        Collider[] cols = Physics.OverlapSphere(origin, radius);
        if (cols.Length > 0)
        {
            GameObject[] nearby = new GameObject[cols.Length];
            for (var i = 0; i < cols.Length; i++)
            {
                nearby[i] = cols[i].gameObject;
            }
            return nearby;
        }
        return null;
    }
}

public class WALK : AIWorkerStates
{
    public WALK(NavMeshAgent _agent, Animator _anim, GameObject _npc)
        : base(_agent, _anim, _npc)
    {
        name = STATE.WALK;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //Findet die nächste gelegenste Ressource
        float lastDis = 15f;
        switch (npc.tag)
        {
            case "Lumberjack":
                //Findet alle am nächsten gelegenen Bäume und fügt die einer Liste hinzu. Und er läuft Random zu diesen Bäumen
                //Vorher hat er immer den selben Baum anvisiert.
                List<GameObject> trees = new List<GameObject>();
                trees.AddRange(GetNearby(npc.transform.position, 15));
                //Debug.Log("Trees länge " + trees.Count);
                for (int i = 0; i < trees.Count; i++)
                {
                    if (trees[i].layer != 8)
                        trees.Remove(trees[i]);
                }
                //Debug.Log("Trees länge nach Clear " + trees.Count);
                GameObject choosenTree = trees[Random.Range(0, trees.Count)];
                //Debug.Log("Choosen Tree: " + choosenTree.name);
                agent.SetDestination(choosenTree.transform.position);
                
                /*for(int i = 0; i < TrackRessources.instance.Trees.Count; i++)
                {
                    Debug.Log(TrackRessources.instance.Trees.Count);
                    GameObject thisTree = TrackRessources.instance.Trees[i];
                    float distance = Vector3.Distance(npc.transform.position, thisTree.transform.position);
                    if (distance < lastDis)
                    {
                        lastDis = distance;
                        agent.SetDestination(thisTree.transform.position);
                    }
                }*/

                break;
            case "Stonemine":
                List<GameObject> stones = new List<GameObject>();
                stones.AddRange(GetNearby(npc.transform.position, 15));
                for (int i = 0; i < stones.Count; i++)
                {
                    if (!stones[i].gameObject.CompareTag("Rock"))
                        stones.Remove(stones[i]);
                }
                GameObject choosenStone = stones[Random.Range(0, stones.Count)];
                agent.SetDestination(choosenStone.transform.position);
                
                
                /*for(int i = 0; i < TrackRessources.instance.Rocks.Count; i++)
                {
                    GameObject thisRock = TrackRessources.instance.Rocks[i];
                    float distance = Vector3.Distance(npc.transform.position, thisRock.transform.position);
                    if (distance < lastDis)
                    {
                        lastDis = distance;
                        agent.SetDestination(thisRock.transform.position);
                    }
                }*/
                break;
            /*case "Farm":
                for(int i = 0; i < TrackRessources.Singelton.FarmField.Count; i++)
                {
                    GameObject thisFarmField = TrackRessources.Singelton.FarmField[i];
                    float distance = Vector3.Distance(npc.transform.position, thisFarmField.transform.position);
                    if (distance < lastDis)
                    {
                        lastDis = distance;
                        agent.SetDestination(thisFarmField.transform.position);
                    }
                }
                break;*/
        }
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            nextState = new WORK(agent, anim, npc);
                stage = EVENT.EXIT;
        }
        else return;
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class WORK : AIWorkerStates
{
    //Abholzen von Ressource, Zeit selber bestimmbar hier im Skript in Zeile 143
    private float timeRemaining;
    public WORK(NavMeshAgent _agent, Animator _anim, GameObject _npc)
        : base(_agent, _anim, _npc)
    {
        name = STATE.WORK;
    }

    public override void Enter()
    {
        if (agent.remainingDistance < 2)
        {
            //anim.SetTrigger("isWorking");
            timeRemaining = 10f;
            agent.isStopped = true;
            base.Enter();
        }
    }

    public override void Update()
    {
        if (timeRemaining > 0) timeRemaining -= Time.deltaTime;
        else
        {
            nextState = new WALKHOME(agent, anim, npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isWorking");
        base.Exit();
    }
}

public class UNLOAD : AIWorkerStates
{
    //Ausladen von Ressource, Zeit selber bestimmbar hier im Skript in Zeile 181
    private float timeRemaining;
    public UNLOAD(NavMeshAgent _agent, Animator _anim, GameObject _npc)
        : base(_agent, _anim, _npc)
    {
        name = STATE.UNLOAD;
    }

    public override void Enter()
    {
        if (agent.remainingDistance < 3)
        {
            //anim.SetTrigger("isUnloading");
            timeRemaining = 10f;
            agent.isStopped = true;
            base.Enter();
        }
    }

    public override void Update()
    {
        if (timeRemaining > 0) timeRemaining -= Time.deltaTime;
        else
        {
            nextState = new WALK(agent, anim, npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isUnloading");
        base.Exit();
    }
}

public class WALKHOME : AIWorkerStates
{
    //Läuft nach Hause/Lager
    public WALKHOME(NavMeshAgent _agent, Animator _anim, GameObject _npc)
        : base(_agent, _anim, _npc)
    {
        name = STATE.WALKHOME;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        agent.SetDestination(npc.transform.parent.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 3)
        {
            nextState = new UNLOAD(agent, anim, npc);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
