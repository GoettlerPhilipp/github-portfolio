using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "CreateLumberjack", menuName = "UtilityAI/AI_Actions/CreateLumberjack")]
public class CreateLumberjack : AI_Action
{

    private bool gotEnoughRessources = false;
    [SerializeField] private GameObject lumberjackPrefab;
    
    public override void Execute(AI_Controller aiBase)
    {
        gotEnoughRessources = false;
        if (aiBase.baseInventory.RessourcesInBase[RessourceTypes.wood] > 5)
        {
            aiBase.baseInventory.RessourcesInBase[RessourceTypes.wood] -= 5;
            gotEnoughRessources = true;
            aiBase.aiBrain.finishedExecutingBestAction = true;
            aiBase.counter = 5;
        }
        else
            aiBase.aiBrain.failedToExecuteBestAction = true;
    }
    
    public override void CreateBuilding(AI_Controller aiBase)
    {
        if(gotEnoughRessources)
        {
            int randomSpot = Random.Range(0, AI_BuildingSpots.Singleton.LumberjackSpots.Count);
            if (AI_BuildingSpots.Singleton.LumberjackSpots.Count > 0)
            {
                GameObject lumberjackInstance = Instantiate(lumberjackPrefab,
                    AI_BuildingSpots.Singleton.LumberjackSpots[randomSpot].transform.position, Quaternion.Euler(AI_BuildingSpots.Singleton.LumberjackSpots[randomSpot].transform.rotation.x, AI_BuildingSpots.Singleton.LumberjackSpots[randomSpot].transform.rotation.y, AI_BuildingSpots.Singleton.LumberjackSpots[randomSpot].transform.rotation.z));
                Destroy(AI_BuildingSpots.Singleton.LumberjackSpots[randomSpot]);
                AI_BuildingSpots.Singleton.LumberjackSpots.Remove(
                    AI_BuildingSpots.Singleton.LumberjackSpots[randomSpot]);
                ChangeLayerOfChildren(lumberjackInstance);
                lumberjackInstance.transform.parent =
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
