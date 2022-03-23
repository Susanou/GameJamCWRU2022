using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    public List<GameObject> contents;
    public Board board;
    public Vector2Int boardCoords;
    public Vector3Int tileCoords;

    public MapTile(Board theBoard) {
        board = theBoard;
        contents = new List<GameObject>();
    }

    public void SetCoords(Vector2Int coords) {
        boardCoords = coords;
        tileCoords = board.CoordsBoardToTilemap(coords);
    }
}
