using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public Camera GameCamera;

    private float cameraMoveSpeed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float vercicalInput = Input.GetAxis("Vertical");

        GameCamera.transform.position = GameCamera.transform.position + GameCamera.transform.forward * cameraMoveSpeed * Time.deltaTime * vercicalInput;
        GameCamera.transform.position = GameCamera.transform.position + GameCamera.transform.right * cameraMoveSpeed * Time.deltaTime * horizontalInput;
    }
}
