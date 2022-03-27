using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    public int visibility;
    public Tilemap fogOfWar;
    public string name;

    [HideInInspector] public List<Vector2Int> visibleTiles;
    [HideInInspector] public List<GameObject> playerUnits;
    [HideInInspector] public List<GameObject> visibleUnits; 

    void Start()
    {
        visibility = 1;
        visibleUnits = new List<GameObject>();
        playerUnits = new List<GameObject>();
    }

}
