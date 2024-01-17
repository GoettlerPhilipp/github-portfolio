using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWolf : MonoBehaviour
{
    //The wolf was a small easteregg in each level, that follows the player
    
    private NavMeshAgent agent;
    private Animator anim;
    private AIWolfState nextState;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        nextState = new Idle(agent, gameObject, anim);
    }

    // Update is called once per frame
    void Update()
    {
        nextState = nextState.Process();
    }
}
