using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GegnerZuteilen : MonoBehaviour
{
    public List<GameObject> FlakVerteidigung = new List<GameObject>();
    public List<GameObject> TankVerteidigung = new List<GameObject>();
    public List<GameObject> PlaneVerteidigung = new List<GameObject>();
    public List<GameObject> SoldierVerteidigung = new List<GameObject>();

    private void FixedUpdate()
    {
        FlakVerteidigung.RemoveAll(x => x == null);
        TankVerteidigung.RemoveAll(x => x == null);
        PlaneVerteidigung.RemoveAll(x => x == null);
        SoldierVerteidigung.RemoveAll(x => x == null);
    }

    private void Awake()
    {
        
        
        foreach (GameObject go in FlakVerteidigung)
        {
            go.AddComponent<GegnerFlak>();                                           //Gibt jedem GameObject in der Liste das Script
            //BoxCollider boxCollider = go.gameObject.AddComponent<BoxCollider>();     //Gibt den GameObject ein BoxCollider, der fürs Triggern zuständig ist + erhöht ist.
            
            
            
        }
        foreach (GameObject go in TankVerteidigung)
        {
            go.AddComponent<GegnerTank>();
            //BoxCollider boxCollider = go.gameObject.AddComponent<BoxCollider>();     
            


        }
        foreach (GameObject go in PlaneVerteidigung)
        {
            go.AddComponent<GegnerPlane>();
            //BoxCollider boxCollider = go.gameObject.AddComponent<BoxCollider>();     
           

        }
        foreach (GameObject go in SoldierVerteidigung)
        {
            go.AddComponent<GegnerSoldier>();
            //BoxCollider boxCollider = go.gameObject.AddComponent<BoxCollider>();     
            //xCollider.center = boxCollider.center + new Vector3(0, 0.2f, 0);
            //xCollider.isTrigger = true;

        }
    }

    
}
