using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This exists to get Sent Messages from the input manager thing and pass them along to other GameObject as needed

public class InputTransmit : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHitSpace() {
        gameManager.ClearTurnSplash();
    }
}
