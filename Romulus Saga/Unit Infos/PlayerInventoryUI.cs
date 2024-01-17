using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI throwerText;
    [SerializeField] private TextMeshProUGUI fighterText;
    [SerializeField] private TextMeshProUGUI horsemanText;

    [Header("Properties")] 
    private bool unitUIOpen;

    [Header("UI")] 
    public GameObject unitUI;
    public PlayerUnitsInventory currentPlayerUnit;
    
    void Update()
    {
        MoveUnitUI();
    }

    private void MoveUnitUI()
    {
        if (currentPlayerUnit != null)
        {
            unitUIOpen = true;
            ChangeText();
            LeanTween.moveY(unitUI.GetComponent<RectTransform>(), 187.5f, 1).setEaseOutExpo();
        }
        else
        {
            LeanTween.moveY(unitUI.GetComponent<RectTransform>(), 50, 1).setEaseOutExpo();
            unitUIOpen = false;
        }
    }
    
    void ChangeText()
    {
        throwerText.text = currentPlayerUnit.UnitsOnMe[UnitData.UnitType.JuvenileThrower].ToString();
        fighterText.text = currentPlayerUnit.UnitsOnMe[UnitData.UnitType.JuvenileFighter].ToString();
        horsemanText.text = currentPlayerUnit.UnitsOnMe[UnitData.UnitType.Horseman].ToString();
    }
}
