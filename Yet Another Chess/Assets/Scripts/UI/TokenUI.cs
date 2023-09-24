using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class TokenUI : MonoBehaviour
{
    public RectTransform panelRectTransform;
    public TokenInfoDisplay tokenInfoDisplay;

    protected Token m_Token;
    protected Canvas m_Canvas;
    protected Camera m_GameCamera;
    public virtual void Show(Token tokenToShow)
    {
        if (tokenToShow == null)
        {
            return;
        }
        m_Token = tokenToShow;
        AdjustPosition();

        m_Canvas.enabled = true;
        tokenInfoDisplay.Show(tokenToShow);
    }

    protected void AdjustPosition()
    {
        if (m_Token == null)
        {
            return;
        }
        Vector3 point = m_GameCamera.WorldToScreenPoint(m_Token.transform.position);
        point.z = 0;
        panelRectTransform.transform.position = point;
    }
    void Start()
    {
        m_GameCamera = Camera.main;
        m_Canvas.enabled = false;
    }

    protected virtual void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPosition();
    }
}
