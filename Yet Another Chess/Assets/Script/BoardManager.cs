using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject chessUnit;
    public GameObject knightToken;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DrawChessBoard());
    }


    IEnumerator DrawChessBoard()
    {
        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j <= 12; j++)
            {

                float jOffset =5*i + 5;
                Instantiate(chessUnit, new Vector3(i * 10, 0, j * 10 + jOffset), chessUnit.transform.rotation);
                yield return new WaitForSeconds(0.05f);
            }
        }
        Instantiate(knightToken, new Vector3(10, 0, 15), knightToken.transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
