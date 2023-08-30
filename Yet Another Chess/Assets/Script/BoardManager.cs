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

    private static Dictionary<string, GameObject> s_units = new Dictionary<string, GameObject>();

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
                s_units.Add(unit.name, unit);
                BoardUnit sriptComponent = unit.GetComponent<BoardUnit>();
                sriptComponent.XYCoordinate = new Vector2Int(x, y);
                sriptComponent.XYZCoordinate = new Vector3Int(x, y, 0 - x - y);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    void SetCurrentToken()
    {

    }
}
