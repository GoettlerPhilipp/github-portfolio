using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateStonemine", menuName = "UtilityAI/AI_Actions/CreateStonemine")]
public class CreateStonemine : AI_Action
{
    [SerializeField] private GameObject stoneminePrefab;
    private bool gotEnoughRessources = false;
    public override void Execute(AI_Controller aiBase)
    {
        aiBase.counter = 5;
        if (aiBase.baseInventory.RessourcesInBase[RessourceTypes.stone] > 5)
        {
            aiBase.baseInventory.RessourcesInBase[RessourceTypes.stone] -= 5;
            gotEnoughRessources = true;
        }
        else
            aiBase.aiBrain.failedToExecuteBestAction = true;
    }
    
    public override void CreateBuilding(AI_Controller aiBase)
    {
        if (gotEnoughRessources)
        {
            int randomSpot = Random.Range(0, AI_BuildingSpots.Singleton.StonemineSpots.Count);
            if (AI_BuildingSpots.Singleton.StonemineSpots.Count > 0)
            {
                GameObject stonemineInstance = Instantiate(stoneminePrefab,
                    AI_BuildingSpots.Singleton.StonemineSpots[randomSpot].transform.position, Quaternion.Euler(AI_BuildingSpots.Singleton.StonemineSpots[randomSpot].transform.rotation.x, AI_BuildingSpots.Singleton.StonemineSpots[randomSpot].transform.rotation.y, AI_BuildingSpots.Singleton.StonemineSpots[randomSpot].transform.rotation.z));
                Destroy(AI_BuildingSpots.Singleton.StonemineSpots[randomSpot]);
                AI_BuildingSpots.Singleton.StonemineSpots.Remove(AI_BuildingSpots.Singleton.StonemineSpots[randomSpot]);
                ChangeLayerOfChildren(stonemineInstance);
                stonemineInstance.transform.parent =
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
