using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWagonMovementFarmer : MonoBehaviour
{
     //Wagenfahrer (Transport) Skript, das auf ihn kommt, damit er in die verschiedenen Stages von AIWagonStates geht. +Energy +UI-Billboard
    [HideInInspector] public NavMeshAgent agent;
    private Animator anim;
    public AIWagonStateFarmer currentState;
    private Transform npc;
    public int maxRessources = 10;
    
    //Für den RoundManager, dass er erkennt, das die Runde "vorgespult" werden muss
    [HideInInspector] public bool finishedWithTasks = false;
    [HideInInspector] public bool parentAndIAreEmpty = false;

    private Dictionary<RessourceTypes, float> foodInInventory;

    private bool currentindex = true;

    private bool isLoaded;
    

    private void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (isLoaded)
        {
            case false:
                foodInInventory = new Dictionary<RessourceTypes, float>()
                {
                    { RessourceTypes.food, 0 }
                };
                npc = GetComponent<Transform>();
            
                agent = this.GetComponent<NavMeshAgent>();
                anim = this.GetComponent<Animator>();
                currentState = new WALKSTORAGE_Farmer(agent, anim, this.gameObject, foodInInventory);
                if (gameObject.layer == 11)
                    BaseInventory.instance.playerWagonDriver.Add(gameObject);
                isLoaded = true;
                break;
        }
        currentState = currentState.Process();
    }

}
