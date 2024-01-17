using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AI_BuildingSpots
{
    
    //It was responsible for where the buildings were constructed
    
    public static AI_BuildingSpots instance;
    private List<GameObject> farmSpots = new List<GameObject>(); 
    public List<GameObject> FarmSpots { get { return farmSpots; } }

    private List<GameObject> stonemineSpots = new List<GameObject>();
    public List<GameObject> StonemineSpots { get { return stonemineSpots; } }

    private List<GameObject> lumberjackSpots = new List<GameObject>();
    public List<GameObject> LumberjackSpots { get { return lumberjackSpots; } }

    public static AI_BuildingSpots Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new AI_BuildingSpots();
                instance.FarmSpots.AddRange(GameObject.FindGameObjectsWithTag("FarmSpot"));
                instance.StonemineSpots.AddRange(GameObject.FindGameObjectsWithTag("StonemineSpot"));
                instance.LumberjackSpots.AddRange(GameObject.FindGameObjectsWithTag("LumberjackSpot"));
            }
            return instance;
        }
    }
}
