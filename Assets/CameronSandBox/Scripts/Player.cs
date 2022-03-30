using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    public int visibility;
    public Tilemap fogOfWar;
    public string name;

    [HideInInspector] public List<Vector2Int> visibleTiles;
    [HideInInspector] public List<GameObject> visibleUnits; 

    [HideInInspector] public List<Vector2Int> playerTiles;
    [HideInInspector] public List<GameObject> playerUnits;

    private int score;

    void Start()
    {
        visibility = 1;

        visibleUnits = new List<GameObject>();
        playerUnits = new List<GameObject>();
        playerTiles = new List<Vector2Int>();
        visibleTiles = new List<Vector2Int>();
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int gain)
    {
        score += gain;
    }

}
