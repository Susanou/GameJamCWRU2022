using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    public int visibility;
    public Tilemap fogOfWar;
    public Camera camera;

    [HideInInspector] public List<GameObject> PlayerUnits;
    [HideInInspector] public List<GameObject> visibleUnits; 

    void Start()
    {
        visibility = 1;
        visibleUnits = new List<GameObject>();
        PlayerUnits = new List<GameObject>();
        PlayerUnits.Add(this.gameObject);
    }
}
