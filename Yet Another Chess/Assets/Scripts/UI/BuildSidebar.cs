using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSidebar : MonoBehaviour
{
    public TokenPlaceButton tokenPlaceButton;
    // Start is called before the first frame update
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

    }

    void OnButtonDraggedOff(Token tokenData)
    {

    }
}
