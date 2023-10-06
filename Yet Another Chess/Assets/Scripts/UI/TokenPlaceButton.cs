using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TokenPlaceButton : MonoBehaviour, IDragHandler
{
    public TextMeshProUGUI buttonText;
    public Image tokenIcon;
    public Button button;
    Token m_Token;
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

    public void OnClick()
    {
        if (buttonTapped != null)
        {
            buttonTapped(m_Token);
        }
    }

    public void InitializeButton(Token tokenData) {
        m_Token = tokenData;
        buttonText.text = m_Token.Name;
        tokenIcon.sprite = m_Token.icon;
        UpdateButton();
    }

    void UpdateButton()
    {
        
    }
}
