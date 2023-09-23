using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUnit : MonoBehaviour
{
    public Vector2Int XYCoordinate;
    public Vector3Int XYZCoordinate;
    private bool _movable;
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
}
