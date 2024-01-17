using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreateUnits : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI soldierText;
    public TextMeshProUGUI tankText;
    public TextMeshProUGUI planeText;
    public TextMeshProUGUI flakText;

    
    
    [Header("Einheiten")]
    public CheckUnits checkUnits;
    

    public int planeNumber;
    public int tankNumber;
    public int soldierNumber;
    public int flakNumber;

    public bool soldierCalled = false;


    public static CreateUnits Instance { get; private set; }
    public Transform spawnPosition;

    [Header("Turn")]
    RoundManager roundManager;

    private void Awake()
    {
        Instance = this;
        
    }

    private void Update()
    {

        soldierText.SetText(soldierNumber.ToString());
        tankText.SetText(tankNumber.ToString());
        planeText.SetText(planeNumber.ToString());
        flakText.SetText(flakNumber.ToString());
    }


    [Header("Einheiten")]
    public GameObject tank;
    public GameObject plane;
    public GameObject soldier;
    public GameObject flak;

    public List<GameObject> createdObjects = new List<GameObject>();

    public void SpawnPlane()
    {
        if (planeNumber > 0)
        {
            if (plane != null)
            {
                Vector3 position = new Vector3(spawnPosition.position.x, spawnPosition.position.y + 0.5f, spawnPosition.position.z);
                GameObject go = (GameObject)Instantiate(plane, position, Quaternion.identity);
                createdObjects.Add(go);
                planeNumber -= 1;
            }
        }
    }
    public void SpawnFlak()
    {
        if (flakNumber > 0)
        {
            if (flak != null)
            {
                Vector3 position = new Vector3(spawnPosition.position.x + 0.5f, spawnPosition.position.y, spawnPosition.position.z);
                GameObject go = (GameObject)Instantiate(flak, position, Quaternion.identity);
                createdObjects.Add(go);
                flakNumber -= 1;
            }
        }
    }
    public void SpawnTank()
    {
        if (tankNumber > 0)
        {
            if (tank != null)
            {
                Vector3 position = new Vector3(spawnPosition.position.x, spawnPosition.position.y - 0.5f, spawnPosition.position.z);
                GameObject go = (GameObject)Instantiate(tank, position, Quaternion.identity);
                createdObjects.Add(go);
                tankNumber -= 1;
            }
        }
    }
    public void SpawnSoldier()
    {
        if (soldierNumber > 0)
        {
            if (soldier != null)
            {
                Vector3 position = new Vector3(spawnPosition.position.x - 0.5f, spawnPosition.position.y, spawnPosition.position.z);
                GameObject go = (GameObject)Instantiate(soldier, position, Quaternion.identity);
                createdObjects.Add(go);
                soldierNumber -= 1;
            }
            
        }
    }

    public void SoldierMinus()
    {
        checkUnits.soldier = this.soldierNumber;
    }
    public void FlakMinus()
    {
        checkUnits.flak = this.flakNumber;
    }
    public void TankMinus()
    {
        checkUnits.tank = this.tankNumber;
    }
    public void PlaneMinus()
    {
        checkUnits.plane = this.planeNumber;
    }
}
