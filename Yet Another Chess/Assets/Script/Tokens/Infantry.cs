using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Infantry : Token
{

    new void Start()
    {
        Debug.Log("Infantry Start");
        Class = "Infantry";
        Info = " Low speed\n Medium attack\n High defense";
        base.Start();

    }
}
