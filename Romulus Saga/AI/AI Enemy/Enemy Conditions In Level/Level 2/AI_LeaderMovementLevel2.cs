using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_LeaderMovementLevel2 : MonoBehaviour
{
    
    //Enemyleader on upperworld conditions for level 2
    
    private AI_LeaderStateLevel2 currentState;
    private NavMeshAgent agent;
    public Dictionary<UnitData.UnitType, int> UnitsOnMe;
    private Animator anim;

    [Header("UnitsOnSpawn")] 
    [SerializeField] private int archerNum = 10;
    [SerializeField] private int swordsmanNum = 10;
    [SerializeField] private int horseRiderNum = 10;
    
    public GameObject juvenileThrower;
    public GameObject juvenileFighter;
    public GameObject horseman;

    private void Awake()
    {
        UnitsOnMe = new Dictionary<UnitData.UnitType, int>()
        {
            { UnitData.UnitType.JuvenileThrower, archerNum },
            { UnitData.UnitType.JuvenileFighter, swordsmanNum },
            { UnitData.UnitType.Horseman, horseRiderNum }
        };
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = new Patrol_Level2(agent, this.gameObject, UnitsOnMe, anim);
        AI_StorageInventory.instance.myLeaders.Add(gameObject);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
        OnStatValueChanged();
    }
    

    #region Billboard

    //Gibt Ressourcen/Energy Werte der dem AI_Billboard Script weiter -> InGame Stats zu sehen
    [SerializeField] private UnitsInventoryBillboard _billboard;
    public delegate void StatValueChangedHandler();
    public event StatValueChangedHandler OnStatValueChanged;


    void UpdateDisplayText()
    {
        _billboard.UpdateUnitsText(this.UnitsOnMe[UnitData.UnitType.JuvenileThrower], this.UnitsOnMe[UnitData.UnitType.JuvenileFighter], this.UnitsOnMe[UnitData.UnitType.Horseman]);
        _billboard.UpdateCurrentState(currentState.name.ToString());
    }

    private void OnEnable()
    {
        OnStatValueChanged += UpdateDisplayText;
    }

    private void OnDisable()
    {
        OnStatValueChanged -= UpdateDisplayText;
    }

    #endregion
}
