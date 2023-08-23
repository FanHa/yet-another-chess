using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUnit : MonoBehaviour
{
    public Vector2Int XYCoordinate;
    public Vector3Int XYZCoordinate;
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
        } else
        {
            renderer.material.color = Color.white;
        }
    }
}
