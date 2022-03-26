using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MapTile location;
    private MoveSelector moveSelector;
    private bool selected = false;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        moveSelector = GameObject.Find("Grid").GetComponent<MoveSelector>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
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

    public void Select() {
        selected = true;
        SetHighlight();
    }

    public void Deselect() {
        selected = false;
        SetHighlight();
    }


    void SetHighlight() {
        if (selected) {
            sprite.color = Color.blue;
        }
        else {
            sprite.color = Color.white;
        }
    }
}
