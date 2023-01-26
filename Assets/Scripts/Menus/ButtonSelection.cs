using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    ButtonMenu buttonMenu;
    void Start()
    {
        buttonMenu = FindObjectOfType<ButtonMenu>();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (GetComponentInChildren<Pointer>())
        {
            Destroy(GetComponentInChildren<Pointer>().gameObject);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Instantiate(buttonMenu.pointer.gameObject, gameObject.transform);
    }
    
}
