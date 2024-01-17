using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIWolfState 
{
    
    public enum STATE{Idle, Follow}
    public enum EVENT{Enter, Update, Exit}

    public STATE name;
    protected EVENT stage;
    protected NavMeshAgent agent;
    protected GameObject npc;
    protected Animator anim;
    protected AIWolfState nextState;

    public AIWolfState(NavMeshAgent _agent, GameObject _npc, Animator _anim)
    {
        agent = _agent;
        npc = _npc;
        anim = _anim;
        stage = EVENT.Enter;
    }

    public virtual void Enter() { stage = EVENT.Update; }
    public virtual void Update() { stage = EVENT.Update; }
    public virtual void Exit() { stage = EVENT.Exit; }

    public AIWolfState Process()
    {
        if(stage == EVENT.Enter) Enter();
        else if(stage == EVENT.Update) Update();
        else if (stage == EVENT.Exit)
        {
            Exit();
            return nextState;
        }
        return this;
    }
    
    

}

public class Idle : AIWolfState
{
    private GameObject romulus;
    public Idle(NavMeshAgent _agent, GameObject _npc, Animator _anim)
        : base(_agent, _npc, _anim)
    {
        name = STATE.Idle;
    }

    public override void Enter()
    {
        romulus = GameObject.Find("Romulus");
        base.Enter();
    }

    public override void Update()
    {
        float distance = Vector3.Distance(npc.transform.position, romulus.transform.position);

        if (distance < 5)
        {
            nextState = new Follow_Wolf(agent, npc, anim);
            stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Follow_Wolf : AIWolfState
{
    private bool getCalledOnce;
    private GameObject romulus;
    public Follow_Wolf(NavMeshAgent _agent, GameObject _npc, Animator _anim)
        : base(_agent, _npc, _anim)
    {
        name = STATE.Follow;
    }

    public override void Enter()
    {
        romulus = GameObject.Find("Romulus");
        Vector3 destination = new Vector3(romulus.transform.position.x + 2, romulus.transform.position.y,
            romulus.transform.position.z);
        agent.SetDestination(romulus.transform.position);
        anim.SetBool("isWalking", true);
        base.Enter();
    }

    public override void Update()
    {
        float distance = Vector3.Distance(romulus.transform.position,npc.transform.position);
        if (distance > 15 && !getCalledOnce)
        {
            anim.SetBool("isRunning", true);
            agent.speed = 13;
            getCalledOnce = true;
        }

        if (distance < 4)
        {
            agent.isStopped = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            agent.speed = 8;
            getCalledOnce = false;
            
        }
        else if(distance > 4)
        {
            agent.isStopped = false;
            anim.SetBool("isWalking", true);
            agent.SetDestination(romulus.transform.position);
        }

        if (romulus == null)
        {
            nextState = new Idle(agent, npc, anim);
            stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    void WalkRandom()
    {
        switch (getCalledOnce)
        {
            case false:
                Vector3 randomPos = new Vector3(npc.transform.position.x + Random.Range(0, 3), npc.transform.position.y,
                    npc.transform.position.z + Random.Range(0, 3));
                agent.SetDestination(randomPos);
                break;
            case true:
                
                break;
        }
        
    }
}
