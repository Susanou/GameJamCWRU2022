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

    public int score = 10;

    private void UpdateCost() {
        float newCost = 2;
        int newScore = 10;

        foreach(GameObject unit in contents) {
            newCost += unit.GetComponent<Unit>().defense;
            newScore += unit.GetComponent<Unit>().unitScore;
        }

        cost = newCost;
        score = newScore;
        //Debug.Log("Coords: " + boardCoords + " => Score = " + score);
    }

    private void UpdateOwner() {
        if (contents.Count > 0) {
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
