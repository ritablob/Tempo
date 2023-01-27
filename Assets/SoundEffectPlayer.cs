using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public void PlaySound(string sfx)
    {
        SoundPlayer.PlaySound(0, sfx);
    }
}
