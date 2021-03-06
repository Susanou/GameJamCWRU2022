using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType 
{
    Generic,
    Vampire,
    Thrall,
    Nightwing,
    Eyewitness,
    VioletWorm,
    Mindflayer,
    Mummy,
    Scarab,
    Sphinx
}

public class Unit : MonoBehaviour
{
    public bool twoMove;
    public bool twoVision;

    public MapTile location;
    private MoveSelector moveSelector;
    private bool selected = false;
    private SpriteRenderer sprite;

    public Player owner;

    public float survivalRate;

    public float attack;
    public float defense;

    public int unitScore;

    public UnitType type;

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
