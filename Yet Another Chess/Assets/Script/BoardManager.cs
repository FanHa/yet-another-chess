using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject chessUnit;
    public GameObject CavalryPrefab;

    private static int unitWidth = 10;
    private static int yOffset = unitWidth / 2;
    private static int xNum = 8;
    private static int yNum = 12;

    void Start()
    {
 
    }

    public static Vector3 GetRealPositionByCoordinate(Vector2 coordinate)
    {
        return new Vector3(coordinate.x * unitWidth, 3, coordinate.y * unitWidth + yOffset * coordinate.x);
    }

    public void DrawChessBoard()
    {
        GameObject chessBoard = GameObject.Find("Chess Board");
        for (int x = 0; x <= xNum; x++)
        {
            for (int y = 0; y <= yNum; y++)
            {

                float offset = yOffset * x;
                GameObject unit = Instantiate(chessUnit, new Vector3(x * unitWidth, 0, y * unitWidth + offset), chessUnit.transform.rotation, chessBoard.transform);
                unit.tag = "Board Unit";
                unit.name = "Board Unit " + x + " " + y;
                BoardUnit sriptComponent = unit.GetComponent<BoardUnit>();
                sriptComponent.XYCoordinate = new Vector2Int(x, y);
                sriptComponent.XYZCoordinate = new Vector3Int(x, y, 0 - x - y);
            }
        }
        var controlToken = Instantiate(CavalryPrefab, GetRealPositionByCoordinate(new Vector2(1, 2)), CavalryPrefab.transform.rotation);
        controlToken.name = "User Knight";
        controlToken.GetComponent<Knight>().BoardPosition = new Vector2Int(1,2);
        controlToken.GetComponent<Knight>().Team = 1;


        var enemyToken = Instantiate(CavalryPrefab, GetRealPositionByCoordinate(new Vector2(3, 4)), CavalryPrefab.transform.rotation);
        enemyToken.GetComponent<Knight>().BoardPosition = new Vector2Int(3, 4);
        enemyToken.GetComponent <Knight>().Team = 2;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCurrentToken()
    {

    }
}
