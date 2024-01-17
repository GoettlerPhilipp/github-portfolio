using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Animals_Movement : MonoBehaviour
{
    
    
    private AI_Animals_State currentState;
    private NavMeshAgent agent;
    public Dictionary<UnitData.UnitType, int> UnitsOnMe;
    private Animator anim;

    private bool currentIndex = true;

    [Header("UnitsOnSpawn")] 
    [SerializeField] private int minRandomRange = 3;
    [SerializeField] private int maxRandomRange = 6;

    public GameObject Wolf;

    private void Awake()
    {
        UnitsOnMe = new Dictionary<UnitData.UnitType, int>()
        {
            { UnitData.UnitType.Wolf, Random.Range(minRandomRange, maxRandomRange) },
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PauseMenuController.instance.currentGameState == GameState.Paused)
            return;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new IDLE(agent, gameObject, UnitsOnMe, anim);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
