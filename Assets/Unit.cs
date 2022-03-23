using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MapTile location;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(MapTile newLocation) {
        if (location != null) location.contents.Remove(gameObject);
        location = newLocation;
        location.contents.Add(gameObject);
    }
}
