using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pathfinder : MonoBehaviour
{
    // I did not write this script. But i worked much with it.
    // I did not write this script. But i worked much with it.
    // I did not write this script. But i worked much with it.
    
    [Header("Properties")]
    public int pathLength;
    public bool hasFoundPath;
    public List<HexaTile> pathTiles = new();
    
    [Header("Lists")]
    public List<HexaTile> openSet = new();
    public List<HexaTile> closedSet = new();

    public List<HexaTile> openSetCalculation = new();
    public List<HexaTile> closedSetCalculation = new();


    private bool startedPathfinding;
    
    public Path FindPath(HexaTile _origin, HexaTile _destination)
    {
        if (!startedPathfinding)
        {
            openSet.Add(_origin);
            startedPathfinding = true;
        }
        
        _origin.node.G = 0;
        var distance = FindObjectOfType<GenerateBattlefield>().offsetX;

        while (openSet.Count > 0)
        {
            openSet.Sort((x, y) => x.node.F.CompareTo(y.node.F));
            var currentTile = openSet[0];
            
            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == _destination)
                return PathBetween(_destination, _origin);
            
            foreach (var neighbour in currentTile.neighbours)
            {
                if (closedSet.Contains(neighbour))
                    continue;

                var costToNeighbour = currentTile.node.G + distance;
                if (costToNeighbour < neighbour.node.G || !openSet.Contains(neighbour))
                {
                    neighbour.node.G = costToNeighbour;
                    neighbour.node.H = Vector3.Distance(_destination.transform.position, neighbour.transform.position);
                    neighbour.node.parent = currentTile;

                    if(!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    public void ResetPathfinding()
    {
        startedPathfinding = false;
        openSet.Clear();
        closedSet.Clear();
        pathTiles.Clear();

        hasFoundPath = false;
        pathLength = 0;
    }
    
    public Path PathBetween(HexaTile _destination, HexaTile _origin)
    {
        var path = MakePath(_destination, _origin);
        return path;
    }


    private Path MakePath(HexaTile _destination, HexaTile _origin)
    {
        var current = _destination;

        while (current != _origin)
        {
            pathTiles.Add(current);
            if (current.node.parent != null)
                current = current.node.parent;
            else
                break;
        }

        pathTiles.Add(_origin);
        pathTiles.Reverse();

        var path = new Path();
        path.hexaTiles = pathTiles.ToArray();

        hasFoundPath = true;
        pathLength = pathTiles.Count - 1;
        return path;
    }
}
