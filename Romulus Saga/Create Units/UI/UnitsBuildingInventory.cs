using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitsBuildingInventory : MonoBehaviour
{
    //Displays the number of units in the building UI.
    [SerializeField] private TextMeshProUGUI archersInInventory;
    [SerializeField] private TextMeshProUGUI speermanInInventory;
    [SerializeField] private TextMeshProUGUI horseRiderInInventory;

    [SerializeField] public BaseInventory choosenInventoryScript;

    public void UpdateUnitsInvetoryText(int archer, int speerman, int horseRider)
    {
        archersInInventory.text = archer.ToString();
        speermanInInventory.text = speerman.ToString();
        horseRiderInInventory.text = horseRider.ToString();
    }
}
