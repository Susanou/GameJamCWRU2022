using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MoveSelector : MonoBehaviour
{
    private int unitsMask;
    private int UIMask;

    private GameObject tileHighlight;
    private List<GameObject> movingPieces = new List<GameObject>();
    private bool selected;

    public Text toggle;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        selected = false;
        unitsMask  = LayerMask.GetMask("Units");
        UIMask  = LayerMask.GetMask("UI");
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
            
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if(hit)
            {
                Vector3 point = hit.point;

                Vector3Int currentCell = GameManager.instance.map.WorldToCell(point);
                foreach (GameObject movingPiece in movingPieces) {
                    GameManager.instance.Move(movingPiece, currentCell);
                }
                selected = true;
                ExitState();
            }
        }
        else {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, distance: Mathf.Infinity);
            if(hit) {
                GameObject isHit = hit.collider.gameObject;
                //Debug.Log(isHit);
                if(isHit.tag == "Unit" && GameManager.instance.DoesBelongToPlayer(isHit)) SelectUnit(isHit);
            }
        }
    }

    void OnHitEnter()
    {
        //Debug.Log("Hit Enter!");
        if(!this.enabled) EnterState();
    }

    public void SelectUnit(GameObject unit) {
        if (movingPieces.Contains(unit)) {
            movingPieces.Remove(unit);
            unit.SendMessage("Deselect");
        }
        else {
            movingPieces.Add(unit);
            unit.SendMessage("Select");
        }
    }

    public void EnterState()
    {
        this.enabled = true;
        selected = false;
        toggle.text = "Selecting Destination";
    }
    
    private void ExitState()
    {
        this.enabled = false;
        foreach (GameObject movingPiece in movingPieces) {
            movingPiece.GetComponent<Unit>().Deselect();
        }
        movingPieces = new List<GameObject>();
        selected = false;
        toggle.text = "Selecting Units";
    }
}
