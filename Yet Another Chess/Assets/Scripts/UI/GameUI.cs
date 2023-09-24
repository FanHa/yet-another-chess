using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Core.Input;
using UnityEngine.EventSystems;

public struct UIPointer
{
    /// <summary>
    /// The pointer info
    /// </summary>
    public PointerInfo pointer;

    /// <summary>
    /// The ray for this pointer
    /// </summary>
    public Ray ray;

    /// <summary>
    /// The raycast hit object into the 3D scene
    /// </summary>
    public RaycastHit? raycast;

    /// <summary>
    /// True if this pointer started over a UI element or anything the event system catches
    /// </summary>
    public bool overUI;
}
[RequireComponent(typeof(Camera))]
public class GameUI : Singleton<GameUI>
{
    public RadiusVisualizerController radiusVisualizerController;
    public event Action<State, State> stateChanged;
    public event Action ghostBecameValid;
    public LayerMask placementBoardUnitMask;

    TokenPlacementGhost m_CurrentToken;
    Camera m_Camera;
    BoardUnit m_CurrentBoardUnit;
    public PlaceInfoUI placeInfoUI;
    public enum State
    {
        Normal,
        Placing,
        Paused,
        GameOver,
        PlacingWithDrag
    }
    public State state { get; private set; }
    public bool isPlacing
    {
        get
        {
            return state == State.Placing || state==State.PlacingWithDrag;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        state = State.Normal;
        m_Camera = GetComponent<Camera>();
    }


    public void SetupRadiusVisualizer(Token token, Transform ghost = null)
    {
        radiusVisualizerController.SetupRadiusVisualizers(token, ghost);
    }

    public void TryPlaceTower(PointerInfo pointerInfo)
    {
        UIPointer pointer = WrapPointer(pointerInfo);

        // Do nothing if we're over UI
        if (pointer.overUI)
        {
            return;
        }
        PlaceToken(pointer);
    }

    protected UIPointer WrapPointer(PointerInfo pointerInfo)
    {
        return new UIPointer
        {
            overUI = IsOverUI(pointerInfo),
            pointer = pointerInfo,
            ray = m_Camera.ScreenPointToRay(pointerInfo.currentPosition)
        };
    }

    protected bool IsOverUI(PointerInfo pointerInfo)
    {
        int pointerId;
        EventSystem currentEventSystem = EventSystem.current;

        // Pointer id is negative for mouse, positive for touch
        var cursorInfo = pointerInfo as MouseCursorInfo;
        var mbInfo = pointerInfo as MouseButtonInfo;
        var touchInfo = pointerInfo as TouchInfo;

        if (cursorInfo != null)
        {
            pointerId = PointerInputModule.kMouseLeftId;
        }
        else if (mbInfo != null)
        {
            // LMB is 0, but kMouseLeftID = -1;
            pointerId = -mbInfo.mouseButtonId - 1;
        }
        else if (touchInfo != null)
        {
            pointerId = touchInfo.touchId;
        }
        else
        {
            throw new ArgumentException("Passed pointerInfo is not a TouchInfo or MouseCursorInfo", "pointerInfo");
        }

        return currentEventSystem.IsPointerOverGameObject(pointerId);
    }

    public void CancelTokenPlacement()
    {
        if (!isPlacing)
        {
            throw new InvalidOperationException("Can't cancel out of token placement when not in the building state.");
        }

        //if (buildInfoUI != null)
        //{
        //    buildInfoUI.Hide();
        //}
        //Destroy(m_CurrentToken.gameObject);
        //m_CurrentToken = null;
        //SetState(State.Normal);
        //DeselectTower();
    }

    public void SetToPlaceMode([NotNull] Token tokenToBuild)
    {
        if (state != State.Normal)
        {
            throw new InvalidOperationException("Trying to enter Place mode when not in Normal mode");
        }

        if (m_CurrentToken != null)
        {
            // Destroy current ghost
            CancelTokenPlacement();
        }
        SetUpToken(tokenToBuild);
        SetState(State.Placing);


    }

    void SetUpToken([NotNull] Token tokenToBuild)
    {
        if (tokenToBuild == null)
        {
            throw new ArgumentNullException("TokenToBuild");
        }

        m_CurrentToken = Instantiate(tokenToBuild.tokenGhostPrefab);
        m_CurrentToken.Initialize(tokenToBuild);
        //m_CurrentToken.Hide();

        //activate build info
        if (placeInfoUI != null)
        {
            placeInfoUI.Show(tokenToBuild);
        }
    }

    void SetState(State newState)
    {
        state = newState;
    }

    public void PlaceToken(UIPointer pointer)
    {
        if (!isPlacing)
        {
            throw new InvalidOperationException("Trying to place token when not in a place Mode");
        }
        if (m_CurrentToken == null || !IsGhostAtValidPosition())
        {
            return;
        }
        PlacementBoardUnitRaycast(ref pointer);
        if (!pointer.raycast.HasValue || pointer.raycast.Value.collider == null)
        {
            CancelGhostPlacement();
            return;
        }
        int cost = m_CurrentTower.controller.purchaseCost;
        bool successfulPurchase = LevelManager.instance.currency.TryPurchase(cost);
        if (successfulPurchase)
        {
            PlaceGhost(pointer);
        }
    }

    protected void PlacementBoardUnitRaycast(ref UIPointer pointer)
    {
        pointer.raycast = null;

        if (pointer.overUI)
        {
            // Pointer is over UI, so no valid position
            return;
        }

        // Raycast onto placement area layer
        RaycastHit hit;
        if (Physics.Raycast(pointer.ray, out hit, float.MaxValue, placementBoardUnitMask))
        {
            pointer.raycast = hit;
        }
    }

    public void CancelGhostPlacement()
    {
        if (!isPlacing)
        {
            throw new InvalidOperationException("Can't cancel out of ghost placement when not in the building state.");
        }

        if (placeInfoUI != null)
        {
            placeInfoUI.Hide();
        }
        Destroy(m_CurrentToken.gameObject);
        m_CurrentToken = null;
        SetState(State.Normal);
        DeselectTower();
    }

    public bool IsGhostAtValidPosition()
    {
        if (!isPlacing)
        {
            throw new InvalidOperationException("Trying to check ghost position when not in a build mode");
        }
        if (m_CurrentToken == null)
        {
            return false;
        }
        if (m_CurrentBoardUnit == null)
        {
            return false;
        }
        return m_CurrentBoardUnit.Placeable();
    }

}
