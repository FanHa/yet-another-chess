using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cavalry : Token
{

    void Start()
    {
        Debug.Log("Calvary Start");
        Class = "Cavalry";
        Info = " High speed\n Medium attack\n Medium defense";

    }
}
