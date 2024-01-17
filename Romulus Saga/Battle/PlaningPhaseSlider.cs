using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using Slider = UnityEngine.UI.Slider;

public class PlaningPhaseSlider : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI leftNumberUnits;
    [SerializeField] public TextMeshProUGUI rightNumberUnits;
    PlaningPhase unitsForSlider;
    public Slider thisSlider;

    private void Awake()
    {
        unitsForSlider = GameObject.Find("BattleMenuController").GetComponent<PlaningPhase>();
        leftNumberUnits.text = unitsForSlider.oldUnit.GetComponent<UnitCombat>().numberOfUnits.ToString();
    }
    public void OnValueChange(float value)
    {
        value = thisSlider.value;
        leftNumberUnits.text = value.ToString();
        rightNumberUnits.text = (thisSlider.maxValue - value).ToString();
    }

    public void AcceptTransfer()
    {
        int numberOnLeft = int.Parse(leftNumberUnits.text);
        int numberOnRight = int.Parse(rightNumberUnits.text);

        unitsForSlider.oldUnit.GetComponent<UnitCombat>().numberOfUnits = numberOnLeft;
        unitsForSlider.newUnit.GetComponent<UnitCombat>().numberOfUnits = numberOnRight;
        unitsForSlider.oldUnit.transform.position = unitsForSlider.characterPos;
        if(unitsForSlider.oldUnit.GetComponent<UnitCombat>().numberOfUnits <= 0)
            Destroy(unitsForSlider.oldUnit);
        else if (unitsForSlider.newUnit.GetComponent<UnitCombat>().numberOfUnits <= 0)
            Destroy(unitsForSlider.newUnit);
    }

    public void DeclineTransfer()
    {
        Destroy(unitsForSlider.newUnit);
        unitsForSlider.oldUnit.transform.position = unitsForSlider.characterPos;
    }
}
