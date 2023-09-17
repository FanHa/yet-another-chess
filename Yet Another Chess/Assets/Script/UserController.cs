using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    public Camera GameCamera;
    public GameObject TokenInfoPanel;
    public GameObject ActionUI;

    private Token _currentToken;


    [SerializeField] private List<BoardUnit> _movableUnits;
    [SerializeField] private List<Token> _attackableTokens; 
    public ActionPhase Phase;

    public enum ActionPhase
    {
        Init,
        Moving,
        Moved,
        Attacking,
        EndTurn,
    }
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

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
                            var token = hit.collider.GetComponentInParent<Token>();
                            if (_currentToken.CanAttackTo(token))
                            {
                                token.AddHP(-25);
                            } else
                            {
                                Debug.Log("cant attack to this token");
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

    public void SetCurrentControlToken(string name)
    {
        var ob = GameObject.Find(name);
        _currentToken = ob.GetComponent<Token>();
        TokenInfoPanel.GetComponent<TokenInfo>().SetName(_currentToken.Class);
        TokenInfoPanel.GetComponent<TokenInfo>().SetInfo(_currentToken.Info);
        _currentToken.ChoosonRing.SetActive(true);
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

    List<Token> GetAttackableTokens()
    {
        var list = new List<Token>();
        var tokens = GameObject.FindGameObjectsWithTag("Token");
        for (int i = 0; i<tokens.Length; i++)
        {
            var token = tokens[i].GetComponent<Token>();
            if (_currentToken.CanAttackTo(token))
            {
                list.Add(token);
            }
        }
        return list;
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
        if (phase == "EndTurn")
        {
            Phase = ActionPhase.EndTurn;
            _currentToken.ChoosonRing.SetActive(false);
            GameController.TurnEnd(_currentToken.Name);
            return;
        }
    }
}
