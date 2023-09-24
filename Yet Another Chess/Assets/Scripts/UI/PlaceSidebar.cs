using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSidebar : MonoBehaviour
{
    public TokenPlaceButton tokenPlaceButton;

    void Start()
    {
        foreach (Token token in LevelManager.instance.tokenLibrary)
        {
            TokenPlaceButton button = Instantiate(tokenPlaceButton, transform);
            button.InitializeButton(token);
            button.buttonTapped += OnButtonTapped;
            button.draggedOff += OnButtonDraggedOff;
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
