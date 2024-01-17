using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIWagonMovement : MonoBehaviour
{
    //Wagon driver (transport) script for getting into the states.
    [HideInInspector]public NavMeshAgent agent;
    private Animator anim;
    public AIWagonStates currentState;
    private Transform npc;
    public int maxRessources = 10;
    
    //We had used a round manager for a while. We only switched to an RTS later on in the project, which is why we sometimes mention rounds.
    [HideInInspector] public bool finishedWithTasks = false;
    [HideInInspector] public bool parentAndIAreEmpty = false;

    private Dictionary<RessourceTypes, int> Ressources;

    private bool currentindex = true;
    

    private void Awake()
    {
        Ressources = new Dictionary<RessourceTypes, int>()
        {
            { RessourceTypes.wood, 0 },
            { RessourceTypes.stone, 0 },
        };
        npc = GetComponent<Transform>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new WALKSTORAGE(agent, anim, this.gameObject, Ressources);
        if (gameObject.layer == 11)
            BaseInventory.instance.playerWagonDriver.Add(gameObject);
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(PauseMenuController.instance.currentGameState == GameState.Paused)
            return;
        currentState = currentState.Process();
    }

}

