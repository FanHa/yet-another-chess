using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject ChessBoard;
    public static GameObject UserController;
    public GameObject CavalryPrefab;
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;
    private static Queue<string> _turns = new Queue<string>();
    // Start is called before the first frame update
    void Start()
    {
        UserController = GameObject.Find("User Controller");
        ChessBoard.GetComponent<BoardManager>().DrawChessBoard();

        var controlInfantryToken = Instantiate(InfantryPrefab, BoardManager.GetRealPositionByCoordinate(new Vector2(1, 0+2)), InfantryPrefab.transform.rotation);
        var infantryTokenName = "User Infantry " + 0;
        controlInfantryToken.GetComponent<Token>().Name = infantryTokenName;
        controlInfantryToken.GetComponent<Token>().BoardPosition = new Vector2Int(1, 0+2);
        controlInfantryToken.GetComponent<Token>().Team = 1;
        _turns.Enqueue(infantryTokenName);

        var controlCavalryToken = Instantiate(CavalryPrefab, BoardManager.GetRealPositionByCoordinate(new Vector2(1, 1 + 2)), CavalryPrefab.transform.rotation);
        var cavalryTokenName = "User Cavalry " + 1;
        controlCavalryToken.GetComponent<Token>().Name = cavalryTokenName;
        controlCavalryToken.GetComponent<Token>().BoardPosition = new Vector2Int(1, 1 + 2);
        controlCavalryToken.GetComponent<Token>().Team = 1;
        _turns.Enqueue(cavalryTokenName);

        var controlArcherToken = Instantiate(ArcherPrefab, BoardManager.GetRealPositionByCoordinate(new Vector2(1, 2 + 2)), ArcherPrefab.transform.rotation);
        var archerTokenName = "User Archer " + 2;
        controlArcherToken.GetComponent<Token>().Name = archerTokenName;
        controlArcherToken.GetComponent<Token>().BoardPosition = new Vector2Int(1, 2 + 2);
        controlArcherToken.GetComponent<Token>().Team = 1;
        _turns.Enqueue(archerTokenName);



        var enemyToken = Instantiate(InfantryPrefab, BoardManager.GetRealPositionByCoordinate(new Vector2(4, 6)), InfantryPrefab.transform.rotation);
        var enemyTokenName = "Enemy Token 0";
        enemyToken.GetComponent<Token>().Name = enemyTokenName;
        enemyToken.GetComponent<Token>().BoardPosition = new Vector2Int(4, 6);
        enemyToken.GetComponent<Token>().Team = 2;
        _turns.Enqueue(enemyTokenName);

        var currentTurn = _turns.Dequeue();
        UserController.GetComponent<UserController>().SetCurrentControlToken(currentTurn);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void TurnEnd(string TokenName)
    {
        _turns.Enqueue(TokenName);
        var nextTurn = _turns.Dequeue();
        UserController.GetComponent<UserController>().SetCurrentControlToken(nextTurn);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
