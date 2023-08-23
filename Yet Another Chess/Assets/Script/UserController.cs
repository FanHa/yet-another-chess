using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public Camera GameCamera;
    public GameObject TokenInfoPanel;

    private Knight currentToken;
    private float cameraMoveSpeed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(TokenInfoPanel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float vercicalInput = Input.GetAxis("Vertical");

        GameCamera.transform.position = GameCamera.transform.position + GameCamera.transform.forward * cameraMoveSpeed * Time.deltaTime * vercicalInput;
        GameCamera.transform.position = GameCamera.transform.position + GameCamera.transform.right * cameraMoveSpeed * Time.deltaTime * horizontalInput;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) )
            {
                var token = hit.collider.GetComponentInParent<Knight>();
                if (token != null)
                {
                    currentToken = token;
                    TokenInfoPanel.GetComponent<TokenInfo>().SetName(token.TokenName);
                    TokenInfoPanel.GetComponent<TokenInfo>().SetInfo(token.Info);
                    TokenInfoPanel.SetActive(true);
                    Debug.Log(token);
                } else
                {
                    TokenInfoPanel.SetActive(false);
                }
                
            }
        }
    }
}
