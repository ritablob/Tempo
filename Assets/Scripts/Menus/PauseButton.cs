using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    PauseMenu pauseMenu;
    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
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
