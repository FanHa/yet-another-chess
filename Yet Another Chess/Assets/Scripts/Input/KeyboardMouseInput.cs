using System.Collections;
using System.Collections.Generic;
using Core.Input;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GameUI))]
public class KeyboardMouseInput : Core.Input.KeyboardMouseInput
{
    GameUI m_GameUI;
    protected override void OnEnable()
    {
        base.OnEnable();

        m_GameUI = GetComponent<GameUI>();

        if (InputController.instanceExists)
        {
            InputController controller = InputController.instance;

            controller.tapped += OnTap;
            controller.mouseMoved += OnMouseMoved;
        }
    }

    protected override void OnDisable()
    {
        if (!InputController.instanceExists)
        {
            return;
        }

        InputController controller = InputController.instance;

        controller.tapped -= OnTap;
        controller.mouseMoved -= OnMouseMoved;
    }

    void OnMouseMoved(PointerInfo pointer)
    {
        // We only respond to mouse info
        var mouseInfo = pointer as MouseCursorInfo;

        if ((mouseInfo != null) && (m_GameUI.isPlacing))
        {
            m_GameUI.TryMoveGhost(pointer, false);
        }
    }

    void OnTap(PointerActionInfo pointer)
    {
        // We only respond to mouse info
        var mouseInfo = pointer as MouseButtonInfo;

        if (mouseInfo != null && !mouseInfo.startedOverUI)
        {
            if (m_GameUI.isPlacing)
            {
                if (mouseInfo.mouseButtonId == 0) // LMB confirms
                {
                    m_GameUI.TryPlaceToken(pointer);
                }
                else // RMB cancels
                {
                    m_GameUI.CancelGhostPlacement();
                }
            }
            //else
            //{
            //    if (mouseInfo.mouseButtonId == 0)
            //    {
            //        // select towers
            //        m_GameUI.TrySelectTower(pointer);
            //    }
            //}
        }
    }
}
