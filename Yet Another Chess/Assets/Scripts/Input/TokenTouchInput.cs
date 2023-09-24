using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;
using UnityEngine.InputSystem.XInput;
using State = GameUI.State;
using Core.Camera;
using Core.Input;


[RequireComponent(typeof(GameUI))]
public class TokenTouchInput : TouchInput
{
    GameUI m_GameUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_GameUI = GetComponent<GameUI>();

        m_GameUI.stateChanged += OnStateChanged;
        m_GameUI.ghostBecameValid += OnGhostBecameValid;

        // Register tap event
        if (InputController.instanceExists)
        {
            InputController.instance.tapped += OnTap;
            InputController.instance.startedDrag += OnStartDrag;
        }

        //// disable pop ups
        //confirmationButtons.canvasEnabled = false;
        //invalidButtons.canvasEnabled = false;

    }

    void OnStateChanged(State previousState, State currentState)
    {
        // Early return for two reasons
        // 1. We are not moving into Build Mode
        // 2. We are not actually touching
        //if (UnityInput.touchCount == 0)
        //{
        //    return;
        //}
        //if (currentState == State.Placing && previousState != State.PlacingWithDrag)
        //{
        //    m_GameUI.MoveGhostToCenter();
        //    confirmationButtons.canvasEnabled = false;
        //    invalidButtons.canvasEnabled = false;
        //}
        //if (currentState == State.BuildingWithDrag)
        //{
        //    m_IsGhostSelected = true;
        //}
    }

    void OnGhostBecameValid()
    {
        // this only needs to be done if the invalid buttons are already on screen
        if (!invalidButtons.canvasEnabled)
        {
            return;
        }
        Vector2 screenPoint = cameraRig.cachedCamera.WorldToScreenPoint(m_GameUI.GetGhostPosition());
        if (!confirmationButtons.canvasEnabled)
        {
            confirmationButtons.canvasEnabled = true;
            invalidButtons.canvasEnabled = false;
            confirmationButtons.TryMove(screenPoint);
        }
    }

    protected virtual void OnTap(PointerActionInfo pointerActionInfo)
    {
        var touchInfo = pointerActionInfo as TouchInfo;
        if (touchInfo != null)
        {
            if (m_GameUI.state == State.Normal && !touchInfo.startedOverUI)
            {
                m_GameUI.TrySelectTower(touchInfo);
            }
            else if (m_GameUI.state == State.Building && !touchInfo.startedOverUI)
            {
                m_GameUI.TryMoveGhost(touchInfo, false);
                if (m_GameUI.IsGhostAtValidPosition() && m_GameUI.IsValidPurchase())
                {
                    confirmationButtons.canvasEnabled = true;
                    invalidButtons.canvasEnabled = false;
                    confirmationButtons.TryMove(touchInfo.currentPosition);
                }
                else
                {
                    invalidButtons.canvasEnabled = true;
                    invalidButtons.TryMove(touchInfo.currentPosition);
                    confirmationButtons.canvasEnabled = false;
                }
            }
        }
    }

    protected virtual void OnStartDrag(PointerActionInfo pointer)
    {
        var touchInfo = pointer as TouchInfo;
        if (touchInfo != null)
        {
            if (m_IsGhostSelected)
            {
                m_GameUI.ChangeToDragMode();
                m_DragPointer = touchInfo;
            }
        }
    }


}
