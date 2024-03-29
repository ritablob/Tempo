using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitCanvasManager : MonoBehaviour
{
    public RhythmKeeper rk;
    public HitCanvas hitCanvas;
    public float floatUpSpeed = 0.2f;
    public float floatUpHeight = 0.3f;
    public float fadeOutSpeed = 5f;

    public void SpawnHitCanvas(Vector3 position, string beatTiming)
    {
        GameObject instance = Instantiate(hitCanvas.gameObject, position, Quaternion.identity);
        //Debug.Log("INstance parent " + instance.transform.parent + ", position "+ instance.transform.position);
        Vector3 direction = instance.transform.position - Camera.main.transform.position;
        transform.LookAt(direction);
        HitCanvas hcanvas = instance.GetComponent<HitCanvas>();
        // turning them all off just in case
        hcanvas.lateChild.SetActive(false);
        hcanvas.perfectChild.SetActive(false);
        hcanvas.earlyChild.SetActive(false);
        hcanvas.missChild.SetActive(false);
        GameObject child;
        if (beatTiming == "Near" && rk.isEarly)
        {
            hcanvas.earlyChild.SetActive(true);
            child = hcanvas.earlyChild;
        }
        else if (beatTiming == "Near" && !rk.isEarly)
        {
            hcanvas.lateChild.SetActive(true);
            child = hcanvas.lateChild;
        }
        else if (beatTiming == "Perfect")
        {
            hcanvas.perfectChild.SetActive(true);
            child = hcanvas.perfectChild;
        }
        else
        {
            //Debug.LogError("miss is spawned");
            hcanvas.missChild.SetActive(true);
            child = hcanvas.missChild;

        }
        StartCoroutine(MoveUp(hcanvas, child, instance));
    }
    IEnumerator MoveUp(HitCanvas hcanvas, GameObject child, GameObject instance)
    {
        if (hcanvas.transform.position.y <= floatUpHeight)
        {
            Vector3 addition = new Vector3(0, floatUpSpeed * Time.deltaTime);
            hcanvas.transform.position += addition;
            yield return new WaitForEndOfFrame();
            StartCoroutine(MoveUp(hcanvas, child, instance));
        }
        else
        {
            StartCoroutine(FadeOut(hcanvas, child, instance));
            yield return null;
        }
    }
    IEnumerator FadeOut(HitCanvas hcanvas, GameObject child, GameObject instance)
    {
        if (child.GetComponent<SpriteRenderer>().color.a > 0)
        {
            Color color1 = child.GetComponent<SpriteRenderer>().color;
            color1.a -= fadeOutSpeed * Time.deltaTime;
            child.GetComponent<SpriteRenderer>().color = color1;
            yield return new WaitForEndOfFrame();
            StartCoroutine(FadeOut(hcanvas, child, instance));
        }
        else
        {
            yield return null;
            Destroy(instance);
        }
    }
}
