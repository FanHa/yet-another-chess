using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Token
{
    int _attackDistance = 3;
    public override bool CanAttackTo(Token target)
    {
        Debug.Log("Call archer attack");
        if (GetDistance(BoardPosition, target.BoardPosition) < _attackDistance) {
            return true;
        } else
        {
            return false;
        }
    }
}
