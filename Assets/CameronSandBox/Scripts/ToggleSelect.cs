using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelect : MonoBehaviour
{
    private MoveSelector moveSelector;
    public bool pickUnits = true;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        moveSelector = GameObject.Find("Grid").GetComponent<MoveSelector>();

        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        pickUnits = true;
        text.text = "Select Units";
    }
}
