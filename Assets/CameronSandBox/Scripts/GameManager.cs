using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public Tilemap map;

    public GameObject player1;
    public GameObject player2;

    private Player currentPlayer;
    private Player otherPlayer;
    private Board board;

    void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Grid").GetComponent<Board>();

        Vector2Int playerBoardPos = board.CoordsTilemapToBoard(GetPlayerCellPosition(player1));
        Debug.Log("Player position=" + playerBoardPos);
        player1.GetComponent<Unit>().MoveTo(board[playerBoardPos.x,playerBoardPos.y]);
        board[playerBoardPos.x,playerBoardPos.y].contents.Add(player1);

        playerBoardPos = board.CoordsTilemapToBoard(GetPlayerCellPosition(player2));
        Debug.Log("Player position=" + playerBoardPos);
        player2.GetComponent<Unit>().MoveTo(board[playerBoardPos.x,playerBoardPos.y]);
        board[playerBoardPos.x,playerBoardPos.y].contents.Add(player2);
        
        currentPlayer = player1.GetComponent<Player>();
        otherPlayer = player2.GetComponent<Player>();

        UpdateFogOfWar(player1.GetComponent<Player>().fogOfWar);
        UpdateFogOfWar(player2.GetComponent<Player>().fogOfWar);

        //disable player 2
        otherPlayer.camera.enabled = false;
        otherPlayer.fogOfWar.gameObject.SetActive(false);
        otherPlayer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3Int GetPlayerCellPosition(GameObject player)
    {
        return map.WorldToCell(player.transform.position);
    }

    public void Move(GameObject movingPiece, Vector3Int tilePoint)
    {
        Debug.Log("board points ");
        board.Move(movingPiece, board.CoordsTilemapToBoard(tilePoint));
        UpdateFogOfWar(currentPlayer.fogOfWar);
        NextPlayer();
    }

    public void NextPlayer()
    {

        Player tmpPlayer = currentPlayer;

        currentPlayer.camera.enabled = false;
        currentPlayer.fogOfWar.gameObject.SetActive(false);
        currentPlayer.gameObject.SetActive(false);

        otherPlayer.camera.enabled = true;
        otherPlayer.fogOfWar.gameObject.SetActive(true);
        otherPlayer.gameObject.SetActive(true);

        currentPlayer = otherPlayer;
        otherPlayer = tmpPlayer;
    }

    void UpdateFogOfWar(Tilemap playerFog)
    {
        Vector3Int currentPlayerTile = playerFog.WorldToCell(currentPlayer.transform.position);

        //Clear the surrounding tiles
        (int,int)[] allNeighbors = new (int,int)[] {(0,0),(-1,0),(1,0),(0,-1),(0,1),(0,0),(0,0)};
        allNeighbors[5] = currentPlayerTile.y %2 == 0 ? (-1,1) : (1,1);
        allNeighbors[6] = currentPlayerTile.y %2 == 0 ? (-1,-1) : (1,-1);

        foreach((int,int) neighbor in allNeighbors) {
            playerFog.SetTile(currentPlayerTile + new Vector3Int(neighbor.Item1, neighbor.Item2, 0), null);
        }

    }
}
