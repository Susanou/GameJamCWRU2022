using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Tilemap map;
    
    public Vector2Int player1Start;
    public Vector2Int player2Start;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    private Player currentPlayer;
    private Player otherPlayer;
    [SerializeField]
    private Board board;

    void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Grid").GetComponent<Board>();

        Vector3 worldPosition = map.CellToWorld(board.CoordsBoardToTilemap(player1Start));
        GameObject player1 = Instantiate(player1Prefab, worldPosition, Quaternion.identity, gameObject.transform);
        player1.GetComponent<Unit>().MoveTo(board[player1Start.x,player1Start.y]);
        board[player1Start.x,player1Start.y].contents.Add(player1);

        worldPosition = map.CellToWorld(board.CoordsBoardToTilemap(player2Start));
        GameObject player2 = Instantiate(player2Prefab, worldPosition, Quaternion.identity, gameObject.transform);
        player2.GetComponent<Unit>().MoveTo(board[player2Start.x,player2Start.y]);
        board[player2Start.x,player2Start.y].contents.Add(player2);


        
        currentPlayer = player1.GetComponent<Player>();
        currentPlayer.fogOfWar = GameObject.Find("Fog Player 1").GetComponent<Tilemap>();
        otherPlayer = player2.GetComponent<Player>();
        otherPlayer.fogOfWar = GameObject.Find("Fog Player 2").GetComponent<Tilemap>();

        UpdateFogOfWar(player1.GetComponent<Player>().fogOfWar);
        //UpdateFogOfWar(player2.GetComponent<Player>().fogOfWar);

        //disable player 2
        otherPlayer.camera.enabled = false;
        otherPlayer.fogOfWar.gameObject.SetActive(false);
        otherPlayer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnit(GameObject unitPrefab, Vector2Int boardCoords, Player player)
    {
        Vector3 worldPosition = map.CellToWorld(board.CoordsBoardToTilemap(boardCoords));
        GameObject newUnit = Instantiate(unitPrefab, worldPosition, Quaternion.identity, gameObject.transform);

        newUnit.GetComponent<Unit>().MoveTo(board[boardCoords.x,boardCoords.y]);
        board[boardCoords.x,boardCoords.y].contents.Add(newUnit);
        player.PlayerUnits.Add(newUnit);
    }

    public Vector3Int GetPlayerCellPosition(GameObject player)
    {
        return map.WorldToCell(player.transform.position);
    }

    public void Move(GameObject movingPiece, Vector3Int tilePoint)
    {
        //Debug.Log("board points ");
        board.Move(movingPiece, board.CoordsTilemapToBoard(tilePoint));
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

        //Debug.Log(currentPlayer.name);

        foreach( GameObject g in currentPlayer.visibleUnits)
        {
            g.SetActive(true);
        }

        UpdateFogOfWar(currentPlayer.fogOfWar);
    }

    void UpdateFogOfWar(Tilemap playerFog)
    {
        Vector3Int currentPlayerTile = playerFog.WorldToCell(currentPlayer.transform.position);
        Vector2Int boardPlayerTile = board.CoordsTilemapToBoard(currentPlayerTile);

        //Clear the surrounding tiles
        (int,int)[] allNeighbors = new (int,int)[] {(0,0),(-1,0),(1,0),(0,-1),(0,1),(0,0),(0,0)};
        allNeighbors[5] = currentPlayerTile.y %2 == 0 ? (-1,1) : (1,1);
        allNeighbors[6] = currentPlayerTile.y %2 == 0 ? (-1,-1) : (1,-1);

        foreach((int,int) neighbor in allNeighbors) {
            playerFog.SetTile(currentPlayerTile + new Vector3Int(neighbor.Item1, neighbor.Item2, 0), null);

            if(board[boardPlayerTile.x + neighbor.Item1, boardPlayerTile.y + neighbor.Item2] != null )
            {
                foreach(GameObject g in board[boardPlayerTile.x + neighbor.Item1, boardPlayerTile.y + neighbor.Item2].contents)
                {
                    currentPlayer.visibleUnits.Add(g);
                    g.SetActive(true);
                }
            }
        }

    }

    public bool DoesBelongToPlayer(GameObject unit)
    {
        return currentPlayer.PlayerUnits.Contains(unit);
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(map.CellToWorld(board.CoordsBoardToTilemap(player1Start)), new Vector3(0.5f,0.5f,0.5f));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(map.CellToWorld(board.CoordsBoardToTilemap(player2Start)), new Vector3(0.5f,0.5f,0.5f));
        
    }

    #endif
}
