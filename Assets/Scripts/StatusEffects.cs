using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public int exposeStacks;
    public int stunStacks;
    public int bleedStacks;
    public int weaknessStacks;
    [SerializeField] GameObject sparkEffect;

    private void Start()
    {
        StartCoroutine(StatusEffectTick());
    }

    private void Update()
    {
        if(bleedStacks > 0)
            GetComponent<PlayerMovement>().HP -= Time.deltaTime * bleedStacks;
    }

    public void AddExpose() { exposeStacks++; }
    public void AddStun() { stunStacks++; }
    public void AddBleed() { bleedStacks++; }
    public void AddWeakness() { weaknessStacks++; }

    IEnumerator StatusEffectTick()
    {
        yield return new WaitForSeconds(5);

        if(stunStacks > 0) { GetComponent<PlayerMovement>().TakeDamage(0, 1, 0, gameObject.transform.position); StartCoroutine(Sparks()); }

        exposeStacks = Mathf.Clamp(exposeStacks - 1, 1, 5);
        stunStacks = Mathf.Clamp(stunStacks - 1, 0, 5);
        bleedStacks = Mathf.Clamp(bleedStacks - 1, 0, 5);
        weaknessStacks = Mathf.Clamp(weaknessStacks - 1, 0, 5);

        StartCoroutine(StatusEffectTick());
    }

    IEnumerator Sparks()
    {
        SoundPlayer.PlaySound(0, "stunned");
        sparkEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sparkEffect.SetActive(false);
    }
}
