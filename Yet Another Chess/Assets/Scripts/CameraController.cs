using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private float speed = 80f;

    private float _movementX;
    private float _movementY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = _movementX;
        float verticalInput = _movementY;

        transform.position = transform.position + transform.forward * speed * Time.deltaTime * verticalInput;
        transform.position = transform.position + transform.right * speed * Time.deltaTime * horizontalInput;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }
}
