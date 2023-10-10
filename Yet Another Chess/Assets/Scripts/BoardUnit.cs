using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BoardUnit : MonoBehaviour, IPlacementBoardUnit
{
    public Vector2Int XYCoordinate;
    public Vector3Int XYZCoordinate;
    private bool _movable;
    private bool _occupied;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovable(bool movable)
    {
        var renderer = GetComponent<Renderer>();
        if (movable) {
            renderer.material.color = Color.green;
            _movable = true;
        } else
        {
            renderer.material.color = Color.white;
            _movable = false;
        }
    }

    public bool GetMovable()
    {
        return _movable;
    }

    public bool Placeable()
    {
        // todo 
        return true;
    }

    public  Vector3 GridToWorld()
    {
        return transform.position;
    }

    public void Occupy()
    {
        _occupied = true;
    }


}
