using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitsHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverBackground;

    [HideInInspector]public enum UnitTypeName{Thrower, Warrior, Horseman, Leader}

    [SerializeField] private UnitTypeName thisUnitType;

    private UnitsCostInfo infoText;
    private GetChoosenBarrack choosenBarrackCosts;
    private void Awake()
    {
        hoverBackground = GameObject.Find("HoverInfo");
        infoText = hoverBackground.GetComponent<UnitsCostInfo>();
        choosenBarrackCosts = FindObjectOfType<GetChoosenBarrack>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        foreach(Transform children in hoverBackground.transform)
            children.gameObject.SetActive(true);

        switch (thisUnitType)
        {
            case UnitTypeName.Thrower:
                infoText.ChangeInfoText(choosenBarrackCosts.choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.wood],
                    choosenBarrackCosts.choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.stone],
                    choosenBarrackCosts.choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.food],
                    choosenBarrackCosts.choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.gold],
                    (int)choosenBarrackCosts.choosenBarrackScript.juvenileThrowerUnit.timer);
                break;
            case UnitTypeName.Warrior:
                infoText.ChangeInfoText(choosenBarrackCosts.choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.wood],
                    choosenBarrackCosts.choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.stone],
                    choosenBarrackCosts.choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.food],
                    choosenBarrackCosts.choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.gold],
                    (int)choosenBarrackCosts.choosenBarrackScript.juvenileFighterUnit.timer);
                break;
            case UnitTypeName.Horseman:
                infoText.ChangeInfoText(choosenBarrackCosts.choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.wood],
                    choosenBarrackCosts.choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.stone],
                    choosenBarrackCosts.choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.food],
                    choosenBarrackCosts.choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.gold],
                    (int)choosenBarrackCosts.choosenBarrackScript.horsemanUnit.timer);
                break;
            case UnitTypeName.Leader:
                infoText.ChangeInfoText(choosenBarrackCosts.choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.wood],
                    choosenBarrackCosts.choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.stone],
                    choosenBarrackCosts.choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.food],
                    choosenBarrackCosts.choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.gold],
                    (int)choosenBarrackCosts.choosenBarrackScript.playerLeader.timer);
                break;
        }
        //hoverBackground.GetComponentInChildren<GameObject>().SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach(Transform children in hoverBackground.transform)
            children.gameObject.SetActive(false);
    }
}
