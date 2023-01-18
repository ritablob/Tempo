using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public int exposeStacks;
    public int stunStacks;

    private void Start()
    {
        StartCoroutine(StatusEffectTick());
    }

    public void AddExpose() { exposeStacks++; }
    public void AddStun() { stunStacks++; }

    IEnumerator StatusEffectTick()
    {
        yield return new WaitForSeconds(5);

        if(stunStacks > 0) { GetComponent<PlayerMovement>().TakeDamage(0, 1, 0, gameObject.transform); }

        exposeStacks = Mathf.Clamp(exposeStacks - 1, 1, 999);
        stunStacks = Mathf.Clamp(stunStacks - 1, 0, 999);

        StartCoroutine(StatusEffectTick());
    }
}
