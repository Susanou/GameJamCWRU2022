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
        descriptions.Add(UnitType.Vampire,"Vampire: Increased attack, and when you take a region that has enemy units in it, you also gain a Thrall unit");
        descriptions.Add(UnitType.Thrall,"Thrall: This unit doesn't do anything special except that you can get more of them.");
        descriptions.Add(UnitType.Nightwing,"Nightwing: High defense high attack and increased radius of tile reveal");
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
