using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    //Skript für die Wolke und wann sie ihren Angriff aktiviert
    [SerializeField] private GameObject lightingStrike;
    private bool lightingStrikeActivated;
    [SerializeField] private float strikeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!lightingStrikeActivated)
        {
            strikeTime -= Time.deltaTime;
            if (strikeTime <= 0)
            {
                lightingStrike.gameObject.SetActive(true);
                lightingStrikeActivated = true;
                Destroy(this.gameObject, 2);
            }
        }
    }
    
}
