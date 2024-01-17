using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWorkerMovement : MonoBehaviour
{
    ////Worker  script for getting into the states.
    private NavMeshAgent agent;
    private Animator anim;
    private AIWorkerStates currentState;


    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new WALK(agent, anim, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}