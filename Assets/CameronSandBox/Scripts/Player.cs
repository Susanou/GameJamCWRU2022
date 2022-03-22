using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> pieces;

    public Player(string name)
    {
        this.name = name;
        pieces = new List<GameObject>();
    }
}
