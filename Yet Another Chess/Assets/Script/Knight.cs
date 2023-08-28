using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Knight : MonoBehaviour
{
    public BoardManager boardManager;
    public Vector2Int BoardPosition = Vector2Int.zero;
    public readonly string TokenName = "Cavalry";
    public readonly string Info = "High speed, medium attack, medium defense";
    [SerializeField] private int _mobility = 3;
    public int Team;
    public int HP;

    
    // Start is called before the first frame update
    void Start()
    {
        HP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Vector2Int targetCoordinate)
    {
        BoardPosition = targetCoordinate;
        var realPosition = BoardManager.GetRealPositionByCoordinate(targetCoordinate);
        transform.position = realPosition;
    }

    public List<Vector2> CanMoveToList()
    {
        return new List<Vector2> { };
    }


    public bool CanAttackTo(Knight target)
    {
        
        return true;
    }

    public bool CanMoveTo(BoardUnit boardUnit)
    {
        var distance = GetDistance(boardUnit.XYZCoordinate, new Vector3Int(BoardPosition.x, BoardPosition.y, 0 - BoardPosition.x - BoardPosition.y));
        Debug.Log(distance);
        Debug.Log(boardUnit.XYCoordinate);
        if (distance <= _mobility)
        {
            return true;
        }
        return false;
    }
    public void AddHP(int hp)
    {
        HP = HP + hp;
        if (HP <= 0)
        {
            // todo 
        }
    }
    public void SetAttackTargetable(bool attackable)
    {
        // todo
    }

    int GetDistance(Vector3Int qrsA, Vector3Int qrsB)
    {
        int dQ = Mathf.Abs(qrsB.x - qrsA.x);
        int dR = Mathf.Abs(qrsB.y - qrsA.y);
        int dS = Mathf.Abs(qrsB.z - qrsA.z);

        return Mathf.Max(dQ, dR, dS);
    }


}
