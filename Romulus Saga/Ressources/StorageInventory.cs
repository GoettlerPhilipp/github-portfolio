using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class StorageInventory : MonoBehaviour
{
    //Inventory of resources buildings
    public static StorageInventory instance;
    public int addWoodEachRound = 0;
    public int addStoneEachRound = 0;
    public int addFoodEachRound = 0;
    
    public float ressourceTimer = 15f;

    private bool getCalledOnce;
    
    
    public Dictionary<RessourceTypes, int> Ressource { get; protected set; }
    public Dictionary<RessourceTypes, float> food { get; protected set; }
    public virtual void InitializeInventory()
    {
        Ressource = new Dictionary<RessourceTypes, int>()
        {
            { RessourceTypes.wood, 0 },
            { RessourceTypes.stone, 0 },
        };

        food = new Dictionary<RessourceTypes, float>()
        {
            { RessourceTypes.food, 0 }
        };
    }
    
    

    private void Awake()
    {
        instance = this;
        InitializeInventory();
    }


    IEnumerator AddRessources()
    {
        yield return new WaitForSecondsRealtime(ressourceTimer);
        switch (this.transform.parent.tag)
        {
            case "Lumberjack":
                Ressource[RessourceTypes.wood]+= addWoodEachRound;
                break;
            case "Stonemine":
                Ressource[RessourceTypes.stone]+= addStoneEachRound;
                break;
            case "Farm":
                food[RessourceTypes.food]+= addFoodEachRound;
                break;
        }
        getCalledOnce = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PauseMenuController.instance.currentGameState == GameState.Paused)
            return;
        if (!getCalledOnce)
        {
            StartCoroutine(AddRessources());
            getCalledOnce = true;
        }
    }
}
