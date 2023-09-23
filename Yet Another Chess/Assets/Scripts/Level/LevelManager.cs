using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    public TokenLibrary tokenLibrary;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
}
