using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TokenInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI tokenName;
    public TextMeshProUGUI description;

    public TextMeshProUGUI health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(Token token)
    {
        DisplayText(tokenName, token.Name);
        DisplayText(description, token.Info);
        //DisplayText(health, string.Format("{0}/{1}", token.CurrentHealth, token.MaxHealth));
    }

    static void DisplayText(TextMeshProUGUI textBox, string text)
    {
        if (textBox != null)
        {
            textBox.text = text;
        }
    }

 
}
