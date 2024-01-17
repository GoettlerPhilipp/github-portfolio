using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleArenaGameManager : MonoBehaviour
{
    public List<GameObject> coverSpawnSpots;
    public List<bool> coverSpawnSpotIsPicked;
    public List<GameObject> coverTypes;
    public List<bool> coverSpawnIsPicked;

    [SerializeField] private int maxCoverSpots;
    private int test = 0;

    private void Awake()
    {
        foreach (var spot in coverSpawnSpots)
            coverSpawnSpotIsPicked.Add(spot);
        for (int i = 0; i < coverSpawnSpotIsPicked.Count; i++)
            coverSpawnSpotIsPicked[i] = false;

        foreach (var cover in coverTypes)
            coverSpawnIsPicked.Add(cover);

        for (int i = 0; i < coverSpawnIsPicked.Count; i++)
            coverSpawnIsPicked[i] = false;


        while (test < maxCoverSpots)
        {
            int pickSpot = Random.Range(0, coverSpawnSpots.Count);
            int pickCover = Random.Range(0, coverTypes.Count);
            if (!coverSpawnIsPicked[pickCover] && !coverSpawnSpotIsPicked[pickSpot])
            {
                coverTypes[pickCover].transform.position = coverSpawnSpots[pickSpot].transform.position;
                coverTypes[pickCover].transform.rotation = coverSpawnSpots[pickSpot].transform.rotation;
                coverSpawnIsPicked[pickCover] = true;
                coverSpawnSpotIsPicked[pickSpot] = true;
                test++;
            }
            
            //Anti Crash System 
            if (maxCoverSpots > coverTypes.Count)
                maxCoverSpots = coverTypes.Count;
            else if (maxCoverSpots > coverSpawnSpots.Count)
                maxCoverSpots = coverSpawnSpots.Count;
            else if(maxCoverSpots > coverTypes.Count && maxCoverSpots > coverSpawnSpots.Count)
                return;
        }

        

    }
}
