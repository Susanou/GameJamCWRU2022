using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSelector : MonoBehaviour
{

    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        selected = false;
        //tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        //tileHighlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function When you press left click
    void OnSelect()
    {

        // not sure why needed but will throw an error otherwise
        if(this.enabled){
            Vector2 ray = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            //Debug.Log(ray);
            
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if(hit)
            {

                Debug.Log("Confirm Move");
                Vector3 point = hit.point;

                Vector3Int currentCell = GameManager.instance.map.WorldToCell(point);
                GameManager.instance.Move(movingPiece, currentCell);
                selected = true;
                
            }
            else
            {
                //tileHighlight.SetActive(false);
            }

        }
    }

    //function when you release left click and have selected something
    void OnConfirm()
    {
        if(selected)
            ExitState();
    }

    public void EnterState(GameObject piece)
    {
        this.enabled = true;
        movingPiece = piece;
    }
    
    private void ExitState()
    {
        this.enabled = false;
        movingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        selected = false;
    }
}
