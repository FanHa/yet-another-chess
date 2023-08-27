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
    public GameObject ActionUI;
    public GameObject BoardManager;

    private Knight _currentToken;
    private float cameraMoveSpeed = 80f;

    private List<BoardUnit> _movableUnits;
    public ActionPhase Phase;

    public enum ActionPhase
    {
        Init,
        Moved,
        Operated,
    }
    // Start is called before the first frame update
    void Awake()
    {
        BoardManager.GetComponent<BoardManager>().DrawChessBoard();
        SetCurrentControlToken("User Knight");
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
                        
                        
                    } else if (hit.collider.CompareTag("Board Unit"))
                    {
                        if (_currentToken != null && Phase == ActionPhase.Init)
                        {
                            var unit = hit.collider.GetComponentInParent<BoardUnit>();
                            if (unit.GetMovable())
                            {
                                _currentToken.MoveTo(unit.XYCoordinate);
                                for (int i = 0; i < _movableUnits.Count; i++)
                                {
                                    _movableUnits[i].SetMovable(false);
                                }
                                ActionUI.SetActive(true) ;
                                Phase = ActionPhase.Moved;
                            }
                            else
                            {
                                Debug.Log("Can not move to this unit");
                            }
                            
                        }
                        

                    }
                } 
  


            }
        }
    }

    void SetCurrentControlToken(string name)
    {
        var ob = GameObject.Find(name);
        Debug.Log(ob);
        _currentToken = ob.GetComponent<Knight>();
        TokenInfoPanel.GetComponent<TokenInfo>().SetName(_currentToken.TokenName);
        TokenInfoPanel.GetComponent<TokenInfo>().SetInfo(_currentToken.Info);
        Phase = ActionPhase.Init;
        _movableUnits = GetMovableUnits(_currentToken.BoardPosition, _currentToken.Mobility);
        for (int i = 0; i < _movableUnits.Count; i++)
        {
            _movableUnits[i].SetMovable(true);
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

    List<BoardUnit> GetMovableUnits(Vector2Int position, int mobility)
    {
        var list = new List<BoardUnit> { };
        var units = GameObject.FindGameObjectsWithTag("Board Unit");
        for (int i = 0; i < units.Length; i++)
        {
            var boardUnit = units[i].GetComponent<BoardUnit>();
            var distance = GetDistance(boardUnit.XYZCoordinate, new Vector3Int(position.x, position.y, 0 - position.x - position.y));
            if (distance <= mobility)
            {
                list.Add(boardUnit);

            }

        }
        return list;
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
