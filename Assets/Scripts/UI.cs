using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour, IPointerClickHandler
{
    // TODO: Fix UI click detection
    public void OnPointerClick(PointerEventData eventData) // When pointer clicks on UI element (e.g. panel, button)
    {
        Debug.Log(eventData);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("UI panel clicked!");
            // Perform your desired actions here
        }
    }
}
