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

        foreach (Vector2Int cell in manager.currentPlayer.playerTiles) {
            reachableTiles.Add(cell);
            reachableTiles.AddRange(board.getNeighbors(cell));
        }
        reachableTiles = reachableTiles.Distinct().ToList();
        
        float totalAttack = CalculateUnitsAttack(attackingUnits);

        foreach (Vector2Int cell in reachableTiles) {
            if (totalAttack >= CalculateCellDefense(cell) || board[cell.x,cell.y].owner == manager.currentPlayer) validTiles.Add(cell);
        }

        return(validTiles);
    }

    public void ConquerRegion(List<GameObject> attackingUnits, Vector2Int targetRegion) {
        Player player = manager.currentPlayer;
        Vector2Int otherPlayerStart = (player.name == "player1") ? manager.player2Start : manager.player1Start;
        
        if (board[targetRegion.x,targetRegion.y].owner != null && board[targetRegion.x,targetRegion.y].owner != player) {
            List<GameObject> toResolve =  new List<GameObject>(board[targetRegion.x,targetRegion.y].GetContents());
            foreach(GameObject unit in toResolve) {
                if (Random.Range(0f,1f) < unit.GetComponent<Unit>().survivalRate) board.Move(unit,otherPlayerStart);
                else manager.RemoveUnit(unit);
            }
        }
    }

    private float CalculateCellDefense(Vector2Int cell) {
        // Optional check board to see if there's anything special in that cell
        float defense = 2;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
