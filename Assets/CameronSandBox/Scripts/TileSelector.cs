using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;
    private GameObject tileHighlight;

    public Tilemap fogOfWar;

    // Start is called before the first frame update
    void Start()
    {
        //tileHighlight = Instantiate(tileHighlightPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
        //tileHighlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Debug.Log(ray);
        
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Debug.Log(hit);
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;

            Vector3Int currentCell = fogOfWar.WorldToCell(point);
            fogOfWar.SetTile(currentCell, null);

        }
        else
        {
            //tileHighlight.SetActive(false);
        }
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        //MoveSelector move = GetComponent<MoveSelector>();
        //move.EnterState(movingPiece);
    }
}
