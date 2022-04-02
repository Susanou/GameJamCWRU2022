using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameManager manager;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Grid").GetComponent<Board>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public List<Vector2Int> FindValidMoves(List<GameObject> attackingUnits) {
        List<Vector2Int> reachableTiles = new List<Vector2Int>();
        List<Vector2Int> validTiles = new List<Vector2Int>();
        bool fastUnits = true;
        bool attackOverride = false;

        // set ups booleans for exceptional cases
        foreach (GameObject unit in attackingUnits) {
            Unit unitScript = unit.GetComponent<Unit>();
            if (!unitScript.twoMove) fastUnits = false;
            if (unitScript.type == UnitType.Mindflayer) attackOverride = true;
            if (unitScript.type == UnitType.Scarab) attackOverride = true;
        }

        foreach (Vector2Int cell in manager.currentPlayer.playerTiles) {
            reachableTiles.Add(cell);
            reachableTiles.AddRange(board.getNeighbors(cell));
        }

        if (fastUnits) {
            foreach (Vector2Int cell in new List<Vector2Int>(reachableTiles)) {
                reachableTiles.AddRange(board.getNeighbors(cell));
            }
        }

        reachableTiles = reachableTiles.Distinct().ToList();
        float totalAttack = CalculateUnitsAttack(attackingUnits);

        foreach (Vector2Int cell in reachableTiles) {
            if (board[cell.x,cell.y].owner == manager.currentPlayer) validTiles.Add(cell);
            else if (attackOverride) validTiles.Add(cell);
            else if (totalAttack >= CalculateCellDefense(cell)) validTiles.Add(cell);
        }

        return(validTiles);
    }

    public void ConquerRegion(List<GameObject> attackingUnits, Vector2Int targetRegion) {
        Player player = manager.currentPlayer;
        Vector2Int otherPlayerStart = (player.name == "player1") ? manager.player2Start : manager.player1Start;

        bool mindControl = false;
        bool createThrall = false;
        bool invade = true;

        if (board[targetRegion.x,targetRegion.y].owner != null && board[targetRegion.x,targetRegion.y].owner != player) {
            List<GameObject> toResolve =  new List<GameObject>(board[targetRegion.x,targetRegion.y].GetContents());

            manager.AddScore(board[targetRegion.x,targetRegion.y].score);

            foreach (GameObject unit in attackingUnits) {
                Unit unitScript = unit.GetComponent<Unit>();
                if (unitScript.type == UnitType.Mindflayer) mindControl = true;
                if (unitScript.type == UnitType.Vampire) createThrall = true;
                if (unitScript.type != UnitType.Scarab) invade = false;
            }

            // Normal Conquering Can Happen and Therefore Does
            if(mindControl) {
                ForcedRetreat(toResolve, targetRegion);
            }
            else if (invade) {
                Debug.Log("invade");
            }
            else {
                foreach(GameObject unit in toResolve) {
                    unit.SetActive(false);
                    if (Random.Range(0f,1f) < unit.GetComponent<Unit>().survivalRate) board.Move(unit,otherPlayerStart);
                    else manager.RemoveUnit(unit);
                }
            }
            // Fallback to override options, first up mind control
            

            // Post attack resolution stuff
            if (createThrall) AddPiece("Thrall", targetRegion, player);

        }
        // Add score when conquering a region
        else if(board[targetRegion.x,targetRegion.y].owner != player){
            manager.AddScore(board[targetRegion.x,targetRegion.y].score);
        }


    }

    private void ForcedRetreat(List<GameObject> retreating, Vector2Int startCell) {
        Player player = retreating[0].GetComponent<Unit>().owner;
        Vector2Int destination;

        List<Vector2Int> validTiles = new List<Vector2Int>();
        foreach (Vector2Int tile in board.getNeighbors(startCell)) {
            if(board[tile.x,tile.y].GetContents().Count == 0) validTiles.Add(tile);
            else if(board[tile.x,tile.y].GetContents()[0].GetComponent<Unit>().owner == player) validTiles.Add(tile);
        }

        if (validTiles.Count >0) destination = validTiles[Random.Range(0,validTiles.Count-1)];
        else destination = (player.name == "player1") ? manager.player1Start : manager.player2Start;

        foreach (GameObject toMove in retreating) board.Move(toMove, destination);
    }

    private float CalculateCellDefense(Vector2Int cell) {
        // Optional check board to see if there's anything special in that cell
        float defense = 1;
        foreach (GameObject unit in board[cell.x,cell.y].GetContents()) {
            defense += unit.GetComponent<Unit>().defense;
        }
        return(defense);
    }

    private float CalculateUnitsAttack(List<GameObject> attackingUnits) {
        float attackTotal = 0;

        foreach(GameObject movingPiece in attackingUnits) {
            attackTotal += movingPiece.GetComponent<Unit>().attack;
        }

        return (attackTotal);
    }

    public void MovePieces(List<GameObject> toMove, Vector2Int destination) {
        foreach (GameObject movingPiece in toMove) {
            board.Move(movingPiece, destination);
            if(movingPiece.GetComponent<Unit>().owner == manager.currentPlayer) {
                manager.UpdateFogOfWar(manager.currentPlayer.fogOfWar, destination);
                if (movingPiece.GetComponent<Unit>().twoVision) {
                    foreach (Vector2Int neighbor in board.getNeighbors(destination)) {
                        manager.UpdateFogOfWar(manager.currentPlayer.fogOfWar, neighbor);
                    }
                }
            }
        }
    }

    public void AddPiece(string name, Vector2Int destination, Player player) {
        GameObject newUnit = Instantiate(Resources.Load(name) as GameObject, manager.transform.position, Quaternion.identity, gameObject.transform);
        player.playerUnits.Add(newUnit);
        newUnit.GetComponent<Unit>().owner = player;
        newUnit.layer = (player.name=="player1") ? LayerMask.NameToLayer("Player1") : LayerMask.NameToLayer("Player2");
        MovePieces(new List<GameObject>{newUnit}, destination);
    }

}
