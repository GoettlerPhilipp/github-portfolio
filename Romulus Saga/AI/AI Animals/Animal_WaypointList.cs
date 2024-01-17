using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_WaypointList : MonoBehaviour
{
    public static Animal_WaypointList instance;
    
    [SerializeField] public List<GameObject> animalWaypoints;
    // Start is called before the first frame update
    void Start()
    {
        animalWaypoints = new List<GameObject>();
        foreach (Transform children in gameObject.transform)
        {
            animalWaypoints.Add(children.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        instance = this;
    }
}
