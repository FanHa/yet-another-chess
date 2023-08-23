using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    public Vector2Int BoardPosition = Vector2Int.zero;
    public readonly string TokenName = "Cavalry";
    public readonly string Info = "High speed, medium attack, medium defense";
    public int Mobility = 2;
    // Start is called before the first frame update
    void Start()
    {
        
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

    public void CanMoveTo(Vector2 targetCoordinate)
    {

    }
}
