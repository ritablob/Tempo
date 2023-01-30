using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    PauseMenu pauseMenu;
    public Sprite clickedPointer;
    private Pointer pointer;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }
    public void PointerSpriteSwitch()
    {
        pointer = gameObject.GetComponentInChildren<Pointer>();
        //Debug.Log("pointer found " + pointer.name);
        pointer.gameObject.GetComponent<Image>().sprite = clickedPointer;
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
        Instantiate(pauseMenu.pointer.gameObject, gameObject.transform);

    }
}
