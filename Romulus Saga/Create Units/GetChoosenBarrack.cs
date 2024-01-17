using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GetChoosenBarrack : MonoBehaviour
{
    
    //Adds units to the queue for buttons
    [SerializeField] public BuildUnits choosenBarrackScript;

    [Header("Herstellung")]
    public GameObject juvenileThrower;
    public GameObject juvenileFighter;
    public GameObject horseman;
    public GameObject playerLeader;

    public Sprite juvenileThrowerImg;
    public Sprite juvenileFighterImg;
    public Sprite horsemanImg;
    public Sprite leaderImg;

    [SerializeField] private int maxUnitsInQueue;

    public bool upgradeIsActive;
    
    public void AddFighterToQueue()
    {
        if (BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] >=
            choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.wood] &&
            BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] >=
            choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.stone] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.food] >=
            choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.food] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.faith] >=
            choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.faith] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.gold] >=
            choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.gold] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.currentResidence] >=
            choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.currentResidence])
        {
            if (this.choosenBarrackScript.queueOfUnits.Count < maxUnitsInQueue)
            {
                this.choosenBarrackScript.AddUnitToQueue(choosenBarrackScript.juvenileFighterUnit);
                
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] -=
                    choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.wood];
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] -=
                    choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.stone];
                BaseInventory.instance.HumanRessources[RessourceTypes.food] -=
                    choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.food];
                BaseInventory.instance.HumanRessources[RessourceTypes.faith] -=
                    choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.faith];
                BaseInventory.instance.HumanRessources[RessourceTypes.gold] -=
                    choosenBarrackScript.juvenileFighterUnit.ressourceCosts[RessourceTypes.gold];
                //BaseInventory.instance.HumanRessources[RessourceTypes.currentResidence] -=
                  //  choosenBarrackScript.speermanUnit.ressourceCosts[RessourceTypes.currentResidence];
            }
            else Debug.Log("Queue is full");
        }
    }
    public void AddThrowerToQueue()
    {
        if (BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] >=
            choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.wood] &&
            BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] >=
            choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.stone] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.food] >=
            choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.food] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.faith] >=
            choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.faith] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.gold] >=
            choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.gold] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.currentResidence] >=
            choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.currentResidence])
        {
            if (this.choosenBarrackScript.queueOfUnits.Count < maxUnitsInQueue)
            {
                this.choosenBarrackScript.AddUnitToQueue(choosenBarrackScript.juvenileThrowerUnit);
                
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] -=
                    choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.wood];
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] -=
                    choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.stone];
                BaseInventory.instance.HumanRessources[RessourceTypes.food] -=
                    choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.food];
                BaseInventory.instance.HumanRessources[RessourceTypes.faith] -=
                    choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.faith];
                BaseInventory.instance.HumanRessources[RessourceTypes.gold] -=
                    choosenBarrackScript.juvenileThrowerUnit.ressourceCosts[RessourceTypes.gold];
            }
            else Debug.Log("Queue is full");
        }
    }
    public void AddHorsemanToQueue()
    {
        if (BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] >=
            choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.wood] &&
            BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] >=
            choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.stone] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.food] >=
            choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.food] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.faith] >=
            choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.faith] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.gold] >=
            choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.gold] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.currentResidence] >=
            choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.currentResidence])
        {
            if (this.choosenBarrackScript.queueOfUnits.Count < maxUnitsInQueue)
            {
                this.choosenBarrackScript.AddUnitToQueue(choosenBarrackScript.horsemanUnit);

                BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] -=
                    choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.wood];
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] -=
                    choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.stone];
                BaseInventory.instance.HumanRessources[RessourceTypes.food] -=
                    choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.food];
                BaseInventory.instance.HumanRessources[RessourceTypes.faith] -=
                    choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.faith];
                BaseInventory.instance.HumanRessources[RessourceTypes.gold] -=
                    choosenBarrackScript.horsemanUnit.ressourceCosts[RessourceTypes.gold];
            }
            else Debug.Log("Queue is full");
        }
    }

    public void AddLeaderToQueue()
    {
        if (BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] >=
            choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.wood] &&
            BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] >=
            choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.stone] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.food] >=
            choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.food] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.faith] >=
            choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.faith] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.gold] >=
            choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.gold] &&
            BaseInventory.instance.HumanRessources[RessourceTypes.currentResidence] >=
            choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.currentResidence])
        {
            if (this.choosenBarrackScript.queueOfUnits.Count < maxUnitsInQueue)
            {
                this.choosenBarrackScript.AddUnitToQueue(choosenBarrackScript.playerLeader);
                
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.wood] -=
                    choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.wood];
                BaseInventory.instance.RessourcesInInventory[RessourceTypes.stone] -=
                    choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.stone];
                BaseInventory.instance.HumanRessources[RessourceTypes.food] -=
                    choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.food];
                BaseInventory.instance.HumanRessources[RessourceTypes.faith] -=
                    choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.faith];
                BaseInventory.instance.HumanRessources[RessourceTypes.gold] -=
                    choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.gold];
            }
            else Debug.Log("Queue is full");
        }
    }

    public void Cheaper()
    {
        choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.wood] = 2;
        choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.stone] = 2;
        choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.food] = 2;
        choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.gold] = 2;
        choosenBarrackScript.playerLeader.ressourceCosts[RessourceTypes.faith] = 2;
        choosenBarrackScript.playerLeader.timer = 2;
    }
}
