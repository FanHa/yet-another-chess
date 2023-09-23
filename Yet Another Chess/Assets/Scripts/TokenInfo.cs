using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TokenInfo : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Content;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(string name)
    {
        Name.text = name;
    }

    public void SetInfo(string info)
    {
        Content.text = info;
    }

}
