using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenPlaceButton : MonoBehaviour, IDragHandler
{
    public TextMeshProUGUI buttonText;
    public Image tokenIcon;
    Token m_token;
    public event Action<Token> buttonTapped;
    public event Action<Token> draggedOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public void InitializeButton(Token tokenData) {
        m_token = tokenData;
        buttonText.text = m_token.Name;
        tokenIcon.sprite = m_token.icon;
        UpdateButton();
    }

    void UpdateButton()
    {

    }
}
