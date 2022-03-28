using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    public List<GameObject> contents;
    public Board board;
    public Vector2Int boardCoords;
    public Vector3Int tileCoords;

    public string owner = null;

    public int cost = 2;

    private void UpdateCost() {
        int newCost = 2;
        foreach(GameObject unit in contents) {
            newCost += unit.GetComponent<Unit>().defense;
        }
        cost = newCost;
    }

    private void UpdateOwner() {
        if (contents.Count > 0) {
            owner = contents[0].GetComponent<Unit>().owner;
        }
        else owner = null;
    }

    public void AddUnit(GameObject unit) {
        contents.Add(unit);
        UpdateCost();
        UpdateOwner();
    }

    public MapTile(Board theBoard) {
        board = theBoard;
        contents = new List<GameObject>();
    }

    public void SetCoords(Vector2Int coords) {
        boardCoords = coords;
        tileCoords = board.CoordsBoardToTilemap(coords);
    }


}
