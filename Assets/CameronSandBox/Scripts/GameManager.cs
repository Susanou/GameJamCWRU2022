using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public Tilemap map;
    public Tilemap fogOfWar;

    public GameObject player;

    private GameObject currentPlayer;

    void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player position=" + GetPlayerCellPosition());
        UpdateFogOfWar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3Int GetPlayerCellPosition()
    {
        return map.WorldToCell(player.transform.position);
    }

    public void Move(GameObject movingPiece, Vector3Int tilePoint)
    {
        movingPiece.transform.position = map.CellToWorld(tilePoint);
        UpdateFogOfWar();
    }

    public void NextPlayer(){

    }

    void UpdateFogOfWar()
    {
        Vector3Int currentPlayerTile = fogOfWar.WorldToCell(player.transform.position);

        //Clear the surrounding tiles
        for(int x=-1; x<= 1; x++)
        {
            for(int y=-1; y<= 1; y++)
            {
                fogOfWar.SetTile(currentPlayerTile + new Vector3Int(x, y, 0), null);
            }

        }

    }
}
