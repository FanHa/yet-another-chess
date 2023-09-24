using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITokenRadiusProvider
{
    float effectRadius { get; }
    Color effectColor { get; }
    Targetter targetter { get; }
}
