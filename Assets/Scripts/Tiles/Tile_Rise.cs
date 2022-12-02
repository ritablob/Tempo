using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Rise : MonoBehaviour
{
    public int x, y;
    bool willRise;
    void Start()
    {
        EventSystem.Instance.AddEventListener($"{x}, {y}", StateChange);
    }

    // Update is called once per frame
    void StateChange(string eventName, object param)
    {
        if(eventName == "Rise") { willRise = true; }
        else if(eventName == "Fall") { willRise = false; }
        else if(eventName == "Update") { UpdateThisTile(); }
        else { Debug.Log($"Invalid event called for {gameObject}"); }
    }

    void UpdateThisTile()
    {
        if (willRise) gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
        else gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
    }

    void Fall()
    {
    }
}
