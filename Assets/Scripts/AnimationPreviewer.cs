using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AnimationPreviewer : MonoBehaviour
{
    public string triggerName;

    private void Start()
    {
        GetComponent<Animator>().SetTrigger(triggerName);
    }
}
