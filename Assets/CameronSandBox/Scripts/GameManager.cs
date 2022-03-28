using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Tilemap map;

    public int maxTurns;
    private int currentTurn;

    public Vector2Int player1Start;
    public Vector2Int player2Start;

    public GameObject P1unitPrefab;
    public GameObject P2unitPrefab;

    public int unitCount;

    public Player p1;
    public Player p2;

    public Player currentPlayer;
    public Player otherPlayer;
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

        currentPlayer = p1;
        otherPlayer = p2;

        Vector3 worldPosition = map.CellToWorld(board.CoordsBoardToTilemap(player1Start));
        for (int i = 0; i < unitCount; i++) {
            GameObject p1unit = Instantiate(P1unitPrefab, worldPosition, Quaternion.identity, gameObject.transform);
            p1.playerUnits.Add(p1unit);
            board.Move(p1unit, player1Start);
        }

        currentPlayer = p2;
        otherPlayer = p1;

        worldPosition = map.CellToWorld(board.CoordsBoardToTilemap(player2Start));
        for (int i = 0; i < unitCount; i++) {
            GameObject p2unit = Instantiate(P2unitPrefab, worldPosition, Quaternion.identity, gameObject.transform);
            p2.playerUnits.Add(p2unit);
            board.Move(p2unit, player2Start);
        }

        currentTurn = 0;

        NextPlayer();
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
        board[boardCoords.x,boardCoords.y].AddUnit(newUnit);
        player.playerUnits.Add(newUnit);
    }

    public Vector3Int GetPlayerCellPosition(GameObject player)
    {
        return map.WorldToCell(player.transform.position);
    }

    public void NextPlayer()
    {
        currentTurn++;

        if(currentTurn > maxTurns)
        {
            Debug.Log("Game Ended!");

            //Show the whole map
            currentPlayer.fogOfWar.gameObject.SetActive(false);
            foreach(GameObject unit in currentPlayer.playerUnits) {
                unit.SetActive(true);
            }

            otherPlayer.fogOfWar.gameObject.SetActive(false);
            foreach(GameObject unit in otherPlayer.playerUnits) {
                unit.SetActive(true);
            }

            //MoveSelector.EndState(currentPlayer);

            currentPlayer = null;
        }
        else{
            Player tmpPlayer = currentPlayer;

            currentPlayer.fogOfWar.gameObject.SetActive(false);
            foreach(GameObject unit in currentPlayer.playerUnits) {
                unit.SetActive(false);
            }

            otherPlayer.fogOfWar.gameObject.SetActive(true);
            foreach(GameObject unit in otherPlayer.playerUnits) {
                unit.SetActive(true);
            }

            currentPlayer = otherPlayer;
            otherPlayer = tmpPlayer;

            foreach(Vector2Int tile in currentPlayer.visibleTiles)
            {
                //Debug.Log(tile);
                foreach(GameObject unit in board[tile.x,tile.y].GetContents()) {
                    unit.SetActive(true);
                }
            }
        }
        
    }

    public void UpdateFogOfWar(Tilemap playerFog, Vector2Int newLocation)
    {
        // Takes in input that is Board Coordinates
        Vector2Int boardPlayerTile = newLocation;
        Vector3Int currentPlayerTile = board.CoordsBoardToTilemap(newLocation);

        //Clear the surrounding tiles
        (int,int)[] allNeighbors = new (int,int)[] {(0,0),(-1,0),(1,0),(0,-1),(0,1),(0,0),(0,0)};
        allNeighbors[5] = currentPlayerTile.y %2 == 0 ? (-1,1) : (1,1);
        allNeighbors[6] = currentPlayerTile.y %2 == 0 ? (-1,-1) : (1,-1);

        foreach((int,int) neighbor in allNeighbors) {
            Vector3Int tileCoords = currentPlayerTile + new Vector3Int(neighbor.Item1, neighbor.Item2, 0);
            playerFog.SetTile(tileCoords, null);

            Vector2Int boardCoords = board.CoordsTilemapToBoard(tileCoords);
            currentPlayer.visibleTiles.Add(board.CoordsTilemapToBoard(tileCoords));
        }

    }

    public void RemoveUnit(GameObject unit) {
        if(p1.playerUnits.Contains(unit)) p1.playerUnits.Remove(unit);
        else if(p2.playerUnits.Contains(unit)) p2.playerUnits.Remove(unit);
        Destroy(unit,0.5f);
    }

    public bool DoesBelongToPlayer(GameObject unit)
    {
        return currentPlayer.playerUnits.Contains(unit);
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
