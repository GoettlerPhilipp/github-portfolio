using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckUnits : MonoBehaviour
{
    public static CheckUnits Instance { get; private set; }

    public int tank;
    public int plane;
    public int soldier;
    public int flak;
    
    public void SetSpawnPosition()
    {
        CreateUnits.Instance.spawnPosition = this.transform;
    }

    public void NumberOfUnits()
    {
        CreateUnits.Instance.soldierNumber = this.soldier;
        CreateUnits.Instance.tankNumber = this.tank;
        CreateUnits.Instance.planeNumber = this.plane;
        CreateUnits.Instance.flakNumber = this.flak;
    }

    public void SetCheckUnits()
    {
        CreateUnits.Instance.checkUnits = this;
    }

    private void Awake()
    {
        Instance = this; 
    }

    public void CreateSoldier()
    {
        
        if (RoundManager.Instance.roundPerTurn > 0)         
        {
            soldier += 1;
            RoundManager.Instance.roundPerTurn -= 1;
        }
    }
    public void CreateTank()
    {

        if (RoundManager.Instance.roundPerTurn > 0)
        {
            tank += 1;
            RoundManager.Instance.roundPerTurn -= 1;
        }
    }
    public void CreatePlane()
    {

        if (RoundManager.Instance.roundPerTurn > 0)
        {
            plane += 1;
            RoundManager.Instance.roundPerTurn -= 1;
        }
    }
    public void CreateFlak()
    {
        if (RoundManager.Instance.roundPerTurn > 0)
        {
            flak += 1;
            RoundManager.Instance.roundPerTurn -= 1;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
            if (collider.CompareTag("tank"))
            {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                RoundManager.Instance.roundPerTurn -= 1;
                tank += 1;
                collider.SendMessage("Destroy");
            }
            else
            {
                collider.SendMessage("ResetPos");
            }
            }
            if (collider.CompareTag("plane"))
            {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                RoundManager.Instance.roundPerTurn -= 1;
                plane += 1;
                collider.SendMessage("Destroy");
            }
            else
            {
                collider.SendMessage("ResetPos");
            }
        }
            if (collider.CompareTag("soldier"))
            {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                RoundManager.Instance.roundPerTurn -= 1;
                soldier += 1;
                collider.SendMessage("Destroy");
            }
            else
            {
                collider.SendMessage("ResetPos");
            }
        }
            if (collider.CompareTag("flak"))
            {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                RoundManager.Instance.roundPerTurn -= 1;
                flak += 1;
                collider.SendMessage("Destroy");
            }
            else
            {
                collider.SendMessage("ResetPos");
            }
        }
    }
}
