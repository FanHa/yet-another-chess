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
    private static Queue<string> _turns = new Queue<string>();
    // Start is called before the first frame update
    void Start()
    {
        UserController = GameObject.Find("User Controller");
        ChessBoard.GetComponent<BoardManager>().DrawChessBoard();
        for (int i=0; i<5; i++)
        {
            var controlToken = Instantiate(InfantryPrefab, BoardManager.GetRealPositionByCoordinate(new Vector2(1, i+2)), InfantryPrefab.transform.rotation);
            var tokenName = "User Token " + i;
            controlToken.GetComponent<Token>().Name = tokenName;
            controlToken.GetComponent<Token>().BoardPosition = new Vector2Int(1, i+2);
            controlToken.GetComponent<Token>().Team = 1;
            _turns.Enqueue(tokenName);
        }
        


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
