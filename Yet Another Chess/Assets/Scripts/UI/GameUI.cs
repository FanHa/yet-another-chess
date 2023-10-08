using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Core.Input;
using UnityEngine.EventSystems;
using Core.Utilities;

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
    /// <summary>
    /// Component that manages the radius visualizers of ghosts and tokens
    /// </summary>
    public RadiusVisualizerController radiusVisualizerController;
    public event Action<State, State> stateChanged;
    public event Action ghostBecameValid;
    public LayerMask placementBoardUnitMask;

    /// <summary>
    /// The radius of the sphere cast 
    /// for checking ghost placement
    /// </summary>
    public float sphereCastRadius = 1;
    public Token currentSelectedToken { get; private set; }

    TokenPlacementGhost m_CurrentToken;
    Camera m_Camera;
    public PlaceInfoUI placeInfoUI;


    IPlacementBoardUnit m_CurrentBoardUnit;

    /// <summary>
    /// Tracks if the ghost is in a valid location and the player can afford it
    /// </summary>
    bool m_GhostPlacementPossible;
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


    /// <summary>
    /// Sets up the radius visualizer for a token or ghost token
    /// </summary>
    public void SetupRadiusVisualizer(Token token, Transform ghost = null)
    {
        radiusVisualizerController.SetupRadiusVisualizers(token, ghost);
    }

    public void TryPlaceToken(PointerInfo pointerInfo)
    {
        UIPointer pointer = WrapPointer(pointerInfo);

        // Do nothing if we're over UI
        if (pointer.overUI)
        {
            return;
        }
        PlaceToken(pointer);
    }

    public void TrySelectToken(PointerInfo info)
    {
        //if (state != State.Normal)
        //{
        //    throw new InvalidOperationException("Trying to select towers outside of Normal state");
        //}
        //UIPointer uiPointer = WrapPointer(info);
        //RaycastHit output;
        //bool hasHit = Physics.Raycast(uiPointer.ray, out output, float.MaxValue, SelectionLayer);
        //if (!hasHit || uiPointer.overUI)
        //{
        //    return;
        //}
        //var controller = output.collider.GetComponent<Token>();
        //if (controller != null)
        //{
        //    SelectTower(controller);
        //}
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

    void SetUpToken([NotNull] Token tokenToPlace)
    {
        if (tokenToPlace == null)
        {
            throw new ArgumentNullException("TokenToBuild");
        }

        m_CurrentToken = Instantiate(tokenToPlace.tokenGhostPrefab);
        m_CurrentToken.Initialize(tokenToPlace);
        //m_CurrentToken.Hide();

        //activate build info
        if (placeInfoUI != null)
        {
            placeInfoUI.Show(tokenToPlace);
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
        PlaceGhost(pointer);
        
    }

    public void TryMoveGhost(PointerInfo pointerInfo, bool hideWhenInvalid = true)
    {
        if (m_CurrentToken == null)
        {
            throw new InvalidOperationException("Trying to move the token ghost when we don't have one");
        }

        UIPointer pointer = WrapPointer(pointerInfo);
        // Do nothing if we're over UI
        if (pointer.overUI && hideWhenInvalid)
        {
            m_CurrentToken.Hide();
            return;
        }
        MoveGhost(pointer, hideWhenInvalid);
    }

    protected void PlaceGhost(UIPointer pointer)
    {
        if (m_CurrentToken == null || !isPlacing)
        {
            throw new InvalidOperationException(
                "Trying to position a token ghost while the UI is not currently in a placing state.");
        }

        MoveGhost(pointer);

        if (m_CurrentBoardUnit != null)
        {
            //bool isPlaceAble = m_CurrentBoardUnit.Placeable();
            bool isPlaceAble = true;

            if (isPlaceAble)
            {
                // Place the ghost
                Token controller = m_CurrentToken.controller;

                Token createdToken = Instantiate(controller);
                //createdToken.Initialize(m_CurrentArea, m_GridPosition);

                CancelGhostPlacement();
            }
        }
    }

    protected void MoveGhost(UIPointer pointer, bool hideWhenInvalid = true)
    {
        if (m_CurrentToken == null || !isPlacing)
        {
            throw new InvalidOperationException(
                "Trying to position a token ghost while the UI is not currently in the placing state.");
        }

        // Raycast onto placement layer
        PlacementAreaRaycast(ref pointer);

        if (pointer.raycast != null)
        {
            MoveGhostWithRaycastHit(pointer.raycast.Value);
        }
        else
        {
            MoveGhostOntoWorld(pointer.ray, hideWhenInvalid);
        }
    }

    /// <summary>
    /// Move ghost with the given ray
    /// </summary>
    protected virtual void MoveGhostOntoWorld(Ray ray, bool hideWhenInvalid)
    {
        m_CurrentBoardUnit = null;

        if (!hideWhenInvalid)
        {
            //RaycastHit hit;
            //// check against all layers that the ghost can be on
            //Physics.SphereCast(ray, sphereCastRadius, out hit, float.MaxValue, placementBoardUnitMask);
            //if (hit.collider == null)
            //{
            //    return;
            //}
            //m_CurrentToken.Show();
            //m_CurrentToken.Move(hit.point, hit.collider.transform.rotation, false);
        }
        else
        {
            m_CurrentToken.Hide();
        }
    }


    protected void PlacementAreaRaycast(ref UIPointer pointer)
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

    // todo
    [ContextMenu("test")]
    protected virtual void MoveGhostWithRaycastHit(RaycastHit raycast)
    {
        // We successfully hit one of our placement areas
        // Try and get a placement area on the object we hit
        m_CurrentBoardUnit = raycast.collider.GetComponent<IPlacementBoardUnit>();

        if (m_CurrentBoardUnit == null)
        {
            Debug.LogError("There is not an IPlacementBoardUnit attached to the collider found on the m_PlacementAreaMask");
            return;
        }
        //m_GridPosition = new IntVector2(0, 0);
        //m_GridPosition = m_CurrentBoardUnit.WorldToGrid(raycast.point, m_CurrentToken.controller.dimensions);
        //TowerFitStatus fits = m_CurrentArea.Fits(m_GridPosition, m_CurrentTower.controller.dimensions);

        m_CurrentToken.Show();
        // TODO Check 
        m_GhostPlacementPossible = true; 
        m_CurrentToken.Move(m_CurrentBoardUnit.GridToWorld(),m_CurrentBoardUnit.transform.rotation,m_GhostPlacementPossible);
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
        DeselectToken();
    }

    public void DeselectToken()
    {
        if (state != State.Normal)
        {
            throw new InvalidOperationException("Trying to deselect tower whilst not in Normal state");
        }
        if (currentSelectedToken != null)
        {
            //currentSelectedToken.removed -= OnTowerDied;
        }

        currentSelectedToken = null;

        //if (selectionChanged != null)
        //{
        //    selectionChanged(null);
        //}
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
        return true;
        //return m_CurrentBoardUnit.Placeable();
    }

}
