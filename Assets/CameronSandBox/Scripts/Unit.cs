using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public MapTile location;
    private MoveSelector moveSelector;
    private bool selected = false;
    private SpriteRenderer sprite;

    public Player owner;
    public string ownerName;

    public float survivalRate;

    public float attack;
    public float defense;

    // Start is called before the first frame update
    void Start()
    {
        if (owner == null) {
            owner = GameObject.Find(ownerName).GetComponent<Player>();
        }
        moveSelector = GameObject.Find("Grid").GetComponent<MoveSelector>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindOwner() {
        owner = GameObject.Find(ownerName).GetComponent<Player>();
    }

    public void MoveTo(MapTile newLocation) {
        // Try not to call this directly. Use Board.Move() which calls this but also does transform stuff
        if (location != null) location.RemoveUnit(gameObject);
        location = newLocation;
        location.AddUnit(gameObject);
        //Debug.Log(location.contents.ToArray()[0]);
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
