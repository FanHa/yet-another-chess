using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSidebar : MonoBehaviour
{
    public TokenPlaceButton tokenPlaceButton;

    Storehouse m_Storehouse;

    void Start()
    {
        m_Storehouse = Storehouse.instance;


        foreach (Token token in LevelManager.instance.tokenLibrary)
        {
            TokenPlaceButton button = Instantiate(tokenPlaceButton, transform);
            button.InitializeButton(token);
            button.buttonTapped += OnButtonTapped;
            button.draggedOff += OnButtonDraggedOff;
            m_Storehouse.IncreaseInventory(token.Class, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonTapped(Token tokenData)
    {
        var gameUI = GameUI.instance;
        if (gameUI.isPlacing)
        {
            gameUI.CancelTokenPlacement();
        }
        gameUI.SetToPlaceMode(tokenData);
    }

    void OnButtonDraggedOff(Token tokenData)
    {

    }
}
