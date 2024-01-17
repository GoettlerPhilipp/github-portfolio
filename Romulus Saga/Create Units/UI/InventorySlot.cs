using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    //InventoryUI Script, in which you drop the units
    public static InventorySlot instance;
    public UnitData.UnitType typeOfUnitDropInventory;
    
    [SerializeField] public GameObject sliderBackground;
    public TextMeshProUGUI sliderNumberLeft;
    public TextMeshProUGUI sliderNumberRight;
    [SerializeField] public Slider unitsSlider;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        instance = this;
        if (this.typeOfUnitDropInventory == draggableItem.typeOfUnit)
        {
            sliderBackground.SetActive(true);
            float playerInv;
            float baseInv;
            switch (typeOfUnitDropInventory)
            {
                case UnitData.UnitType.JuvenileThrower:
                    sliderNumberLeft.text = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileThrower].ToString();
                    sliderNumberRight.text = BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileThrower].ToString();
                    playerInv = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileThrower];
                    baseInv = BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileThrower];
                    unitsSlider.value = CalculateValue(baseInv, playerInv);
                    break;
                case UnitData.UnitType.JuvenileFighter:
                    sliderNumberLeft.text = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileFighter].ToString();
                    sliderNumberRight.text = BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileFighter].ToString();
                    playerInv = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileFighter];
                    baseInv = BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileFighter];
                    unitsSlider.value = CalculateValue(baseInv, playerInv);
                    break;
                case UnitData.UnitType.Horseman:
                    sliderNumberLeft.text = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.Horseman].ToString();
                    sliderNumberRight.text = BaseInventory.instance.UnitsInBase[UnitData.UnitType.Horseman].ToString();
                    playerInv = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.Horseman];
                    baseInv = BaseInventory.instance.UnitsInBase[UnitData.UnitType.Horseman];
                    unitsSlider.value = CalculateValue(baseInv, playerInv);
                    break;
            }
            
            
        }
    }
    
    float CalculateValue(float baseInv, float playerInv)
    {
        if(baseInv == 0 && playerInv == 0)
            return 0.5f;
        float test1 = (baseInv - playerInv) / 2;
        float test2 = 1 / (baseInv + playerInv);
        float test3 = test1 * test2 + 0.5f;
        return test3;
    }
}
