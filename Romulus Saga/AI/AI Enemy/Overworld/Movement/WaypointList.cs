using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaypointList : MonoBehaviour
{
    //I placed waypoints on the map, to which the enemyleaders randomly walked
    
    public static WaypointList instance;
    [SerializeField]public List<GameObject> waypoints;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<GameObject>();
        foreach (Transform children in gameObject.transform)
        {
            if(!waypoints.Contains(children.gameObject))
                waypoints.Add(children.gameObject);
        }
    }
    private void Awake()
    {
        instance = this;
    }
}
