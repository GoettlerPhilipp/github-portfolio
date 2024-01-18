using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterSec : MonoBehaviour
{
    public float countTime;
    public List<GameObject> Objects = new List<GameObject>();


    private void Awake()
    {
        Invoke("Counter", countTime);
    }

     void Counter()
    {
        foreach (GameObject go in Objects)
        {
            go.SetActive(true);
            
        }
    }
}
