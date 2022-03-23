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
    private Board board;

    void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Grid").GetComponent<Board>();

        Vector2Int playerBoardPos = board.CoordsTilemapToBoard(GetPlayerCellPosition());
        Debug.Log("Player position=" + playerBoardPos);
        player.GetComponent<Unit>().MoveTo(board[playerBoardPos.x,playerBoardPos.y]);
        board[playerBoardPos.x,playerBoardPos.y].contents.Add(player);

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
        Debug.Log("board points ");
        board.Move(movingPiece, board.CoordsTilemapToBoard(tilePoint));
        UpdateFogOfWar();
    }

    public void NextPlayer(){

    }

    void UpdateFogOfWar()
    {
        Vector3Int currentPlayerTile = fogOfWar.WorldToCell(player.transform.position);

        //Clear the surrounding tiles
        (int,int)[] allNeighbors = new (int,int)[] {(0,0),(-1,0),(1,0),(0,-1),(0,1),(0,0),(0,0)};
        allNeighbors[5] = currentPlayerTile.y %2 == 0 ? (-1,1) : (1,1);
        allNeighbors[6] = currentPlayerTile.y %2 == 0 ? (-1,-1) : (1,-1);

        foreach((int,int) neighbor in allNeighbors) {
            fogOfWar.SetTile(currentPlayerTile + new Vector3Int(neighbor.Item1, neighbor.Item2, 0), null);
        }

    }
}
