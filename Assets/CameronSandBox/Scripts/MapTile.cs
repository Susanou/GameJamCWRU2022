using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    private List<GameObject> contents;
    public Board board;
    public Vector2Int boardCoords;
    public Vector3Int tileCoords;

    public Player owner = null;

    public float cost = 2;

    private void UpdateCost() {
        float newCost = 2;
        foreach(GameObject unit in contents) {
            newCost += unit.GetComponent<Unit>().defense;
        }
        cost = newCost;
    }

    private void UpdateOwner() {
        if (contents.Count > 0) {
            if (contents[0].GetComponent<Unit>().owner == null) contents[0].GetComponent<Unit>().FindOwner();
            owner = contents[0].GetComponent<Unit>().owner;
            if (!owner.playerTiles.Contains(this.boardCoords)) owner.playerTiles.Add(this.boardCoords);
        }
        else if (owner != null) {
            owner.playerTiles.Remove(this.boardCoords);
            owner = null;
        }
    }

    public bool IsEmpty() {
        return (contents.Count == 0);
    }

    public void RemoveUnit(GameObject unit) {
        contents.Remove(unit);
        UpdateCost();
        UpdateOwner();
    }

    public void AddUnit(GameObject unit) {
        contents.Add(unit);
        UpdateCost();
        UpdateOwner();
    }

    public List<GameObject> GetContents() {
        return(contents);
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
