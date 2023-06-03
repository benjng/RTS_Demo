using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public static bool isPointingUI;

    // TODO: Fix UI click detection

    // public void OnPointerDown(PointerEventData eventData)
    // {
        
    //     // Debug.Log(eventData);
    //     // if (eventData.button == PointerEventData.InputButton.Left){}
    //     Debug.Log("UI panel clicked!");  
    // }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointingUI = true;
        Debug.Log("Pointer entered UI");
    }

    public void OnPointerExit(PointerEventData eventData){
        isPointingUI = false;
        Debug.Log("Pointer exited UI");
    }
}
