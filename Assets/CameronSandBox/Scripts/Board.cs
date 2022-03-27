using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{

    private MapTile[,] tiles;
    private MapTile oceanTile; // Prefab for oceantile, which we consider out of bounds stuff to be
    private Tilemap map;

    public int numCols;
    public int numRows;
    // Using a single number for each of these will technically result in a skew diamond overall map
    // But I figure we can just overshoot and have the camera not reach the edge
    // the excess will just be empty ocean tiles that you never see so. whatever

    void Start() {
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        
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
        //foreach(GameObject movingPiece in movingPieces) {

        movingPiece.GetComponent<Unit>().MoveTo(this[newCoords.x,newCoords.y]);
        movingPiece.transform.position = map.CellToWorld(tilePoint);

        //}
    }

    public Vector2[] getNeighbors(Vector2 centerCoord) {
        Vector2[] neighbors = new Vector2[6];
        neighbors[0] = new Vector2(centerCoord.x-1,centerCoord.y);
        neighbors[1] = new Vector2(centerCoord.x+1,centerCoord.y);
        neighbors[2] = new Vector2(centerCoord.x,centerCoord.y-1);
        neighbors[3] = new Vector2(centerCoord.x,centerCoord.y+1);
        neighbors[4] = new Vector2(centerCoord.x+1,centerCoord.y-1);
        neighbors[5] = new Vector2(centerCoord.x-1,centerCoord.y+1);
        return(neighbors);
    }
}
