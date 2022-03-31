using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{

    private MapTile[,] tiles;
    private MapTile oceanTile; // Prefab for oceantile, which we consider out of bounds stuff to be
    private Tilemap map;
    private Tilemap highlight;
    private GameManager gameManager;

    public int numCols;
    public int numRows;

    public Vector2Int player1Start;
    public Vector2Int player2Start;

    public TileBase highlightTile;


    // Using a single number for each of these will technically result in a skew diamond overall map
    // But I figure we can just overshoot and have the camera not reach the edge
    // the excess will just be empty ocean tiles that you never see so. whatever

    void Start() {
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        highlight = GameObject.Find("Highlight Layer").GetComponent<Tilemap>();
        
        tiles = new MapTile[numRows, numCols];
        int tileCount = 0;
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                tiles[row, col] = new MapTile(this);
                tiles[row, col].SetCoords(new Vector2Int(col,numRows - 1 - row));

                tileCount += 1;
            }
            if (numCols %2 == 0) tileCount += 1;
        }
        player1Start = gameManager.player1Start;
        player2Start = gameManager.player2Start;
    }

    public MapTile this[int x, int y]
    {
        get {
            if (x < 0 || y < 0 || x >= numCols || y >= numRows)
                return oceanTile;
            return tiles[numCols - y - 1, x];
        }
        set {
            if (x < 0 || y < 0 || x >= numCols || y >= numRows)
                throw new System.Exception("Coordinates [" + x + ", " + y + "] are out of bounds but you're trying to set them");
            tiles[numCols - y - 1, x] = value;
        }
    }

    public void HighlightTiles(List<Vector2Int> tiles) {
        foreach (Vector2Int tile in tiles) {
            highlight.SetTile(CoordsBoardToTilemap(tile), highlightTile);
        }
    }

    public void ClearHighlight() {
        highlight.ClearAllTiles();
    }

    public Vector3Int CoordsBoardToTilemap(Vector2Int boardCoords) {
        boardCoords += new Vector2Int(-13,-11);
        Vector3Int newCoords = Vector3Int.zero;
        newCoords.y = (int) boardCoords.y;
        int xOffset = (int) Mathf.Floor(boardCoords.y / 2f);
        newCoords.x = (int) boardCoords.x + xOffset;
        return(newCoords);
    }

    public Vector2Int CoordsTilemapToBoard(Vector3Int tileCoords) {
        Vector2Int newCoords = Vector2Int.zero;
        newCoords.y = tileCoords.y;
        int xOffset = (int) Mathf.Floor(tileCoords.y / 2f);
        newCoords.x = tileCoords.x - xOffset;
        newCoords += new Vector2Int(13,11);
        return(newCoords);
    }

    public void Move(GameObject movingPiece, Vector2Int newCoords) {
        Vector3Int tilePoint = CoordsBoardToTilemap(newCoords);
        MapTile oldLocation = null;

        if (movingPiece.GetComponent<Unit>().location != null) {
            oldLocation = movingPiece.GetComponent<Unit>().location;
        }
        movingPiece.GetComponent<Unit>().MoveTo(this[newCoords.x,newCoords.y]);

        Arrange(this[newCoords.x,newCoords.y].GetContents(),map.CellToWorld(tilePoint));
        if (oldLocation != null) {
            Arrange(this[oldLocation.boardCoords.x,oldLocation.boardCoords.y].GetContents(),map.CellToWorld(oldLocation.tileCoords));
        }
    }

    void Arrange(List<GameObject> toArrage, Vector2 destination) {
        float xMin = destination.x - 0.5f;
        float xRange = 1f;
        float xInc = xRange/(toArrage.Count+1);

        for (int i = 0; i < toArrage.Count; i++) {
            toArrage[i].transform.position = new Vector3(xMin + xInc*(i+1),destination.y-0.05f,0);
        }
    }

    public Vector2Int[] getNeighbors(Vector2Int centerCoord) {
        Vector2Int[] neighbors = new Vector2Int[6];
        neighbors[0] = new Vector2Int(centerCoord.x-1,centerCoord.y);
        neighbors[1] = new Vector2Int(centerCoord.x+1,centerCoord.y);
        neighbors[2] = new Vector2Int(centerCoord.x,centerCoord.y-1);
        neighbors[3] = new Vector2Int(centerCoord.x,centerCoord.y+1);
        neighbors[4] = new Vector2Int(centerCoord.x+1,centerCoord.y-1);
        neighbors[5] = new Vector2Int(centerCoord.x-1,centerCoord.y+1);
        return(neighbors);
    }

    public List<GameObject> getTileContent(Vector2Int coordinates)
    {
        return this[coordinates.x, coordinates.y].GetContents();
    }
}
