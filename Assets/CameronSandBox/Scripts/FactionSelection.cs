using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionSelection : MonoBehaviour
{

    public Dropdown p1Faction;
    public Dropdown p2Faction;
    public InputField turnNumber;

    // Start is called before the first frame update
    void Start()
    {

        GameManager.p1faction = "Vampire";
        GameManager.p2faction = "Vampire";
        GameManager.maxTurns = 20; // 10 turns for each player

        p1Faction.onValueChanged.AddListener(delegate
        {
            player1Faction(p1Faction);
        });

        p2Faction.onValueChanged.AddListener(delegate
        {
            player2Faction(p2Faction);
        });


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void player1Faction(Dropdown faction)
    {

        Debug.Log(faction.value);

        switch (faction.value)
        {
            case 0:
                GameManager.p1faction = "Vampire";
                break;
            case 1:
                GameManager.p1faction = "Eldritch";
                break;
            case 2:
                GameManager.p1faction = "Mummy";
                break;
            default:
                GameManager.p1faction = "Generic";
                break;
        }
    }

    public void player2Faction(Dropdown faction)
    {
        switch (faction.value)
        {
            case 0:
                GameManager.p2faction = "Vampire";
                break;
            case 1:
                GameManager.p2faction = "Eldritch";
                break;
            case 2:
                GameManager.p2faction = "Mummy";
                break;
            default:
                GameManager.p2faction = "Generic";
                break;
        }
    }

    public void maxTurn(InputField turns)
    {
        GameManager.maxTurn = Int32.Parse(turns.value);
    }
}
