using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyButtons;

public class RessourcesInBuildingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ressourceText;
    [SerializeField] private Image ressourceImage;

    public AIWagonMovement woodWagonDriver;
    public AIWagonMovement stoneWagonDriver;
    public AIWagonMovementFarmer farmerWagonDriver;
    public StorageInventory inventoryOfBuilding;

    [Header("Images")] 
    [SerializeField] private Sprite food;
    [SerializeField] private Sprite wood;
    [SerializeField] private Sprite stone;
    
    
    // Update is called once per frame
    void Update()
    {
        if (farmerWagonDriver != null)
        {
            ressourceImage.sprite = food;
            ressourceText.text = inventoryOfBuilding.food[RessourceTypes.food].ToString("F0");
        }
        else if (woodWagonDriver != null)
        {
            ressourceImage.sprite = wood;
            ressourceText.text = inventoryOfBuilding.Ressource[RessourceTypes.wood].ToString();
        }
        else if (stoneWagonDriver != null)
        {
            ressourceImage.sprite = stone;
            ressourceText.text = inventoryOfBuilding.Ressource[RessourceTypes.stone].ToString();
        }
    }
}
