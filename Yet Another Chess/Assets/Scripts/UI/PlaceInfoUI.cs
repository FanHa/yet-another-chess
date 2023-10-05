using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum AnimationState
{
    /// <summary>
    /// The UI is completely hidden
    /// </summary>
    Hidden,

    /// <summary>
    /// The UI is animation to be shown
    /// </summary>
    Showing,

    /// <summary>
    /// the UI is completely shown
    /// </summary>
    Shown,

    /// <summary>
    /// The UI is animating 
    /// </summary>
    Hiding
}

[RequireComponent(typeof(TokenUI))]
public class PlaceInfoUI : MonoBehaviour
{
    protected TokenUI m_TokenUI;
    protected Canvas m_Canvas;

    AnimationState m_State;


    protected virtual void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        m_TokenUI = GetComponent<TokenUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Show(Token controller)
    {
        m_TokenUI.Show(controller);
    }

    public virtual void Hide()
    {
        //if (m_State == AnimationState.Hidden)
        //{
        //    return;
        //}
        //m_TokenUI.Hide();
        //anim.Play(hideClipName);
        //m_State = anim[hideClipName].normalizedTime < 1 ? AnimationState.Hiding :
        //    AnimationState.Hidden;
    }
}
