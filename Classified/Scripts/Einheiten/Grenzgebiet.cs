using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenzgebiet : MonoBehaviour
{
    public List<GameObject> EnemyArea = new List<GameObject>();

    private void Start()
    {
        EnemyArea.RemoveAll(x => x == null);
        foreach (GameObject go in EnemyArea)
        {
                go.SetActive(true);
        }
        
    }

    
}
