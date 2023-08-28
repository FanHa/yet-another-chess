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

    [SerializeField] private List<BoardUnit> _movableUnits;
    [SerializeField] private List<Knight> _attackableTokens; 
    public ActionPhase Phase;

    public enum ActionPhase
    {
        Init,
        Moving,
        Moved,
        Attacking,
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
                        if (Phase == ActionPhase.Attacking)
                        {
                            var token = hit.collider.GetComponentInParent<Knight>();
                            if (_currentToken.CanAttackTo(token))
                            {
                                token.AddHP(-10);
                            }
                        }
                        
                        
                    } else if (hit.collider.CompareTag("Board Unit"))
                    {
                        if (_currentToken != null && Phase == ActionPhase.Moving)
                        {
                            var unit = hit.collider.GetComponentInParent<BoardUnit>();
                            if (unit.GetMovable())
                            {
                                _currentToken.MoveTo(unit.XYCoordinate);
                                HideBoardUnitCanMoveTo();
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
        _currentToken = ob.GetComponent<Knight>();
        TokenInfoPanel.GetComponent<TokenInfo>().SetName(_currentToken.TokenName);
        TokenInfoPanel.GetComponent<TokenInfo>().SetInfo(_currentToken.Info);
        Phase = ActionPhase.Init;

    }

    void ShowAttackableEnemy()
    {
        for (int i = 0; i< _attackableTokens.Count ;i++)
        {
            _attackableTokens[i].SetAttackTargetable(true);
        }
    }
    void ShowBoardUnitCanMoveTo()
    {
        for (int i = 0; i < _movableUnits.Count; i++)
        {
            var boardUnit = _movableUnits[i].GetComponent<BoardUnit>();
            boardUnit.SetMovable(true);
        }
    }

    void HideBoardUnitCanMoveTo()
    {
        for (int i = 0; i < _movableUnits.Count; i++)
        {
            var boardUnit = _movableUnits[i].GetComponent<BoardUnit>();
            boardUnit.SetMovable(false);
        }
    }

    List<BoardUnit> GetMovableUnits()
    {
        var list = new List<BoardUnit>();
        var units = GameObject.FindGameObjectsWithTag("Board Unit");
        for (int i = 0; i < units.Length; i++)
        {
            var boardUnit = units[i].GetComponent<BoardUnit>();
            if (_currentToken.CanMoveTo(boardUnit))
            {
                list.Add(boardUnit);
            }
            

        }
        return list;
    }

    List<Knight> GetAttackableTokens()
    {
        var list = new List<Knight>();
        var tokens = GameObject.FindGameObjectsWithTag("Token");
        for (int i = 0; i<tokens.Length; i++)
        {
            var token = tokens[i].GetComponent<Knight>();
            if (_currentToken.CanAttackTo(token))
            {
                list.Add(token);
            }
        }
        return list;
    }

   

    int GetDistance(Vector3Int qrsA, Vector3Int qrsB)
    {
        int dQ = Mathf.Abs(qrsB.x - qrsA.x);
        int dR = Mathf.Abs(qrsB.y - qrsA.y);
        int dS = Mathf.Abs(qrsB.z - qrsA.z);

        return Mathf.Max(dQ, dR, dS);
    }

    public void SetPhase(string phase)
    {
        if (phase == "Attacking")
        {
            Phase = ActionPhase.Attacking;
            _attackableTokens = GetAttackableTokens();
            ShowAttackableEnemy();
            return;
        }

        if (phase == "Moving")
        {
            Phase = ActionPhase.Moving;
            _movableUnits = GetMovableUnits();
            ShowBoardUnitCanMoveTo();
            return;
        }
    }
}
