using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverText : MonoBehaviour
{
    Dictionary<UnitType, string> descriptions = new Dictionary<UnitType, string>();

    private Text myText;

    // Start is called before the first frame update
    void Start()
    {
        myText = gameObject.transform.GetChild(0).GetComponent<Text>();
        gameObject.SetActive(false);

        descriptions.Add(UnitType.Generic,"Generic: This is a placeholder unit, which you should not be seeing");
        descriptions.Add(UnitType.Vampire,"Vampire: Increased attack, and when it takes a region that has enemy units, you gain a Thrall unit");
        descriptions.Add(UnitType.Thrall,"Thrall: This unit doesn't do anything special except that you can get more of them. Dies pretty easily too");
        descriptions.Add(UnitType.Nightwing,"Nightwing: High defense high attack and increased radius of tile reveal");
        descriptions.Add(UnitType.Eyewitness,"Eyewitness: Low defense, but increased vision radius and move speed, and very hard to kill");
        descriptions.Add(UnitType.VioletWorm,"Violet Worm: Decent attack, excellent defense, but not likely to walk away from a fight");
        descriptions.Add(UnitType.Mindflayer,"Mindflayer: Very low attack, but they can take any region by forcing enemy units to retreat to a nearby cell, or back home if they're surrounded");
        descriptions.Add(UnitType.Mummy,"Mummy: Powerful units that are never killed by an attack");
        descriptions.Add(UnitType.Scarab,"Scarab: These units have no attack or defense, but they can invade an occupied tile, earning points for the tile without affecting existing units");
        descriptions.Add(UnitType.Sphinx,"Sphinx: Two attack, two defense, two move. But this unit isn't likely to survive an attack, and it can't see for the full range of its movement");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVisible(UnitType type, Player player) {
        gameObject.SetActive(true);
        myText.text = descriptions[type];
        myText.color = player.name == "player1" ? new Color(0.4f,0.88f,0.94f) : new Color(0.98f,0.14f,0.45f);
    }

    public void SetInvisible() {
        myText.text = "";
        gameObject.SetActive(false);
    }
}
