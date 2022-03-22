using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;
    private GameObject tileHighlight;

    public Tilemap map;
    public Tilemap fogOfWar; // DO I need this?    

    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        //tileHighlight = Instantiate(tileHighlightPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
        //tileHighlight.SetActive(false);
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {   

    }

    
    void OnSelect()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Debug.Log("trying to select");
        
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
        if(hit)
        {

            //Debug.Log("hit");
            Vector3 point = hit.point;

            Vector3Int currentCell = map.WorldToCell(point);
            Debug.Log(currentCell);
            if(currentCell == GameManager.instance.GetPlayerCellPosition())
            {
                //would rather check the position of the player on the board rather than using world positions
                Debug.Log("Confirm hit player");
                selected = true;
            }

        }
        else
        {
            //tileHighlight.SetActive(false);
        }
    }

    void OnConfirm()
    {
        if(selected)
        {
            ExitState(GameManager.instance.player);
        }
    }

    public void EnterState()
    {
        enabled = true;
        selected = false;
    }

    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        //tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        move.EnterState(movingPiece);
        selected = false;
    }
}
