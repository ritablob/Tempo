using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    [SerializeField] string soundName;
    bool isOnFloor;

    void Update()
    {
        if(transform.position.y <= 0 && !isOnFloor)
        {
            SoundPlayer.PlaySound(1, soundName);
            isOnFloor = true;
            return;
        }

        if(transform.position.y > 0)
            isOnFloor = false;
    }
}
