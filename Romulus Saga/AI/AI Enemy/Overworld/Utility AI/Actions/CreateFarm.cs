using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateFarm", menuName = "UtilityAI/AI_Actions/CreateFarm")]
public class CreateFarm : AI_Action
{
    public GameObject farmPrefab;
    private bool gotEnoughRessources = false;
    
    public override void Execute(AI_Controller aiBase)
    {
        aiBase.counter = 5;
        if (aiBase.baseInventory.FoodInBase[RessourceTypes.food] > 5)
        {
            aiBase.baseInventory.FoodInBase[RessourceTypes.food] -= 5;
            gotEnoughRessources = true;
        }
        else
            aiBase.aiBrain.failedToExecuteBestAction = true;
    }


    public override void CreateBuilding(AI_Controller aiBase)
    {
        if (gotEnoughRessources)
        {
            int randomSpot = Random.Range(0, AI_BuildingSpots.Singleton.FarmSpots.Count);
            if (AI_BuildingSpots.Singleton.FarmSpots.Count > 0)
            {
                GameObject farmInstance = Instantiate(farmPrefab, AI_BuildingSpots.Singleton.FarmSpots[randomSpot].transform.position,
                    Quaternion.Euler(AI_BuildingSpots.Singleton.FarmSpots[randomSpot].transform.rotation.x, AI_BuildingSpots.Singleton.FarmSpots[randomSpot].transform.rotation.y, AI_BuildingSpots.Singleton.FarmSpots[randomSpot].transform.rotation.z));
                Destroy(AI_BuildingSpots.Singleton.FarmSpots[randomSpot]);
                AI_BuildingSpots.Singleton.FarmSpots.Remove(AI_BuildingSpots.Singleton.FarmSpots[randomSpot]);
                ChangeLayerOfChildren(farmInstance);
                farmInstance.transform.parent =
                    GameObject.FindWithTag("EnemyMainBase").transform.parent.GetChild(0).transform;
            }
        }
        gotEnoughRessources = false;
    }
    
    void ChangeLayerOfChildren(GameObject go)
    {
        go.layer = 12;

        foreach (Transform child in go.transform)
        {
            if(child == null)
                continue;
            ChangeLayerOfChildren(child.gameObject);
        }
    }
}
