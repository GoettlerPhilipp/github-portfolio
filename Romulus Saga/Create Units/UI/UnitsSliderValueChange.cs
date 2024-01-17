using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSliderValueChange : MonoBehaviour
{
    //One of my favourite features when you move a slider in the inventory is that the values go up and down on both sides. Like a scale
    
    private UnitData.UnitType typeOfUnit;
    public GameObject toManyUnitsText;

    static float lerp(float minValue, float maxValue, float value) => (1f - value) * minValue + value * maxValue;
    static float invLerp(float minValue, float maxValue, float n) => (n - minValue) / (maxValue - minValue);
    public void OnValueChanged(float value)
    {
        typeOfUnit = InventorySlot.instance.typeOfUnitDropInventory;
        switch (typeOfUnit)
        {
            case UnitData.UnitType.JuvenileThrower:
                value = InventorySlot.instance.unitsSlider.value;
                float totalThrower = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileThrower] +
                              BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileThrower];
                var valueOfSliderThrower = invLerp(0f, 1f, value);
                var baseInventoryThrower = (int)Mathf.Round(lerp(0f, totalThrower, valueOfSliderThrower));
                var playerInventoryThrower = totalThrower - baseInventoryThrower;
                InventorySlot.instance.sliderNumberLeft.text = playerInventoryThrower.ToString();
                InventorySlot.instance.sliderNumberRight.text = baseInventoryThrower.ToString();
                break;
            case UnitData.UnitType.JuvenileFighter:
                value = InventorySlot.instance.unitsSlider.value;
                float totalFighter = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileFighter] +
                              BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileFighter];
                var valueOfSliderFighter = invLerp(0f, 1f, value);
                var baseInventoryFighter = (int)Mathf.Round(lerp(0f, totalFighter, valueOfSliderFighter));
                var playerInventoryFighter = totalFighter - baseInventoryFighter;
                InventorySlot.instance.sliderNumberLeft.text = playerInventoryFighter.ToString();
                InventorySlot.instance.sliderNumberRight.text = baseInventoryFighter.ToString();
                break;
            case UnitData.UnitType.Horseman:
                value = InventorySlot.instance.unitsSlider.value;
                float totalHorseman = BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.Horseman] +
                                     BaseInventory.instance.UnitsInBase[UnitData.UnitType.Horseman];
                var valueOfSliderHorseman = invLerp(0f, 1f, value);
                var baseInventoryHorseman = (int)Mathf.Round(lerp(0f, totalHorseman, valueOfSliderHorseman));
                var playerInventoryHorseman = totalHorseman - baseInventoryHorseman;
                InventorySlot.instance.sliderNumberLeft.text = playerInventoryHorseman.ToString();
                InventorySlot.instance.sliderNumberRight.text = baseInventoryHorseman.ToString();
                break;
        }
    }

    public void AcceptUnitValueChange()
    {
        int numberOfLeftunits = int.Parse(InventorySlot.instance.sliderNumberLeft.text);
        int numberOfRightUnits = int.Parse(InventorySlot.instance.sliderNumberRight.text);
        switch (typeOfUnit)
        {
            case UnitData.UnitType.JuvenileThrower:
                if (BaseInventory.instance.closestPlayerScript.maxUnitsInInventory >= numberOfLeftunits +
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileFighter] +
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.Horseman])
                {
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileThrower] =
                        numberOfLeftunits;
                    BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileThrower] = numberOfRightUnits;
                }
                else
                    StartCoroutine(ActivateTextForSeconds());
                
                break;
            case UnitData.UnitType.JuvenileFighter:
                if (BaseInventory.instance.closestPlayerScript.maxUnitsInInventory >= numberOfLeftunits +
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileThrower] +
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.Horseman])
                {
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileFighter] =
                        numberOfLeftunits;
                    BaseInventory.instance.UnitsInBase[UnitData.UnitType.JuvenileFighter] = numberOfRightUnits;
                }
                else
                    StartCoroutine(ActivateTextForSeconds());

                break;
            case UnitData.UnitType.Horseman:
                if (BaseInventory.instance.closestPlayerScript.maxUnitsInInventory >= numberOfLeftunits +
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileFighter] +
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.JuvenileThrower])
                {
                    BaseInventory.instance.closestPlayerScript.UnitsOnMe[UnitData.UnitType.Horseman] =
                        numberOfLeftunits;
                    BaseInventory.instance.UnitsInBase[UnitData.UnitType.Horseman] = numberOfRightUnits;
                }
                else
                    StartCoroutine(ActivateTextForSeconds());
                break;
        }
    }

    IEnumerator ActivateTextForSeconds()
    {
        toManyUnitsText.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        toManyUnitsText.SetActive(false);
    }
}
