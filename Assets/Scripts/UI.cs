using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public static bool isPointingUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Pointer entered UI");
        isPointingUI = true;
    }

    public void OnPointerExit(PointerEventData eventData){
        // Debug.Log("Pointer exited UI");
        isPointingUI = false;
    }
}
