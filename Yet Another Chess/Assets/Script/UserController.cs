using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UserController : MonoBehaviour
{
    public Camera GameCamera;
    public GameObject TokenInfoPanel;

    private Knight currentToken;
    private float cameraMoveSpeed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(TokenInfoPanel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float vercicalInput = Input.GetAxis("Vertical");

        GameCamera.transform.position = GameCamera.transform.position + GameCamera.transform.forward * cameraMoveSpeed * Time.deltaTime * vercicalInput;
        GameCamera.transform.position = GameCamera.transform.position + GameCamera.transform.right * cameraMoveSpeed * Time.deltaTime * horizontalInput;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) )
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Token"))
                    {
                        var token = hit.collider.GetComponentInParent<Knight>();
                        currentToken = token;
                        TokenInfoPanel.GetComponent<TokenInfo>().SetName(token.TokenName);
                        TokenInfoPanel.GetComponent<TokenInfo>().SetInfo(token.Info);
                        TokenInfoPanel.SetActive(true);
                        ShowBoardUnitCanMoveTo(token.BoardPosition, token.Mobility);
                        
                    } else if (hit.collider.CompareTag("Board Unit"))
                    {
                        if (currentToken != null)
                        {
                            var unit = hit.collider.GetComponentInParent<BoardUnit>();
                            // todo movable
                            currentToken.MoveTo(unit.XYCoordinate);
                            currentToken = null;
                        }
                        

                    } else
                    {
                        TokenInfoPanel.SetActive(false);

                    }
                } 
  


            }
        }
    }

    void ShowBoardUnitCanMoveTo(Vector2Int position, int mobility)
    {
 
        var units = GameObject.FindGameObjectsWithTag("Board Unit");
        for (int i = 0; i < units.Length; i++)
        {
            var boardUnit = units[i].GetComponent<BoardUnit>();
            var distance = GetDistance(boardUnit.XYZCoordinate, new Vector3Int(position.x, position.y, 0 - position.x - position.y));
            if (distance <= mobility)
            {
                boardUnit.SetMovable(true);

            }

        }
    }

    void HideBoardUnitCanMoveTo(Vector2Int position, int mobility)
    {

        var units = GameObject.FindGameObjectsWithTag("Board Unit");
        for (int i = 0; i < units.Length; i++)
        {
            var boardUnit = units[i].GetComponent<BoardUnit>();
            var distance = GetDistance(boardUnit.XYZCoordinate, new Vector3Int(position.x, position.y, 0 - position.x - position.y));
            if (distance <= mobility)
            {
                boardUnit.SetMovable(false);

            }

        }
    }

    int GetDistance(Vector3Int qrsA, Vector3Int qrsB)
    {
        int dQ = Mathf.Abs(qrsB.x - qrsA.x);
        int dR = Mathf.Abs(qrsB.y - qrsA.y);
        int dS = Mathf.Abs(qrsB.z - qrsA.z);

        return Mathf.Max(dQ, dR, dS);
    }
}
