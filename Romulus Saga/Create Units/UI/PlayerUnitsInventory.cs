using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsInventory : MonoBehaviour
{
    //Units in the player inventory
    
    public Dictionary<UnitData.UnitType, int> UnitsOnMe;

    public int archer = 10;
    public int fighter;
    public int horse;

    public int maxUnitsInInventory = 40;
    public int currentUnitCounter;

    public GameObject juvenileThrower;
    public GameObject juvenileFighter;
    public GameObject horseman;

    private void Awake()
    {
        UnitsOnMe = new Dictionary<UnitData.UnitType, int>()
        {
            { UnitData.UnitType.JuvenileThrower, archer },
            { UnitData.UnitType.JuvenileFighter, fighter },
            { UnitData.UnitType.Horseman, horse }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentUnitCounter = UnitsOnMe[UnitData.UnitType.JuvenileThrower] +
                             UnitsOnMe[UnitData.UnitType.JuvenileFighter] + 
                             UnitsOnMe[UnitData.UnitType.Horseman];
        //OnStatValueChanged();
    }

    /*#region MyRegion

    //Gibt Einheiten/Energy Werte dem UnitsInventoryBillboard Script weiter -> InGame Stats zu sehen
    [SerializeField] private UnitsInventoryBillboard _billboard;
    public delegate void StatValueChangedHandler();
    public event StatValueChangedHandler OnStatValueChanged;


    void UpdateDisplayText()
    {
        _billboard.UpdateUnitsText(this.UnitsOnMe[UnitData.UnitType.JuvenileThrower], this.UnitsOnMe[UnitData.UnitType.JuvenileFighter], this.UnitsOnMe[UnitData.UnitType.Horseman]);
    }

    private void OnEnable()
    {
        OnStatValueChanged += UpdateDisplayText;
    }

    private void OnDisable()
    {
        OnStatValueChanged -= UpdateDisplayText;
    }

    #endregion*/
}
