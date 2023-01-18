using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCanvasManager : MonoBehaviour
{
    public HitCanvas hitCanvas;
    public float popupLength = 2f;


    public void SpawnHitCanvas(Vector3 position, float beatPercentage)
    {
        GameObject instance = Instantiate(hitCanvas.gameObject, position, Quaternion.identity);
        Debug.Log("INstance parent " + instance.transform.parent + ", position "+ instance.transform.position);
        Vector3 direction = instance.transform.position - Camera.main.transform.position;
        transform.LookAt(direction);

        HitCanvas hcanvas = instance.GetComponent<HitCanvas>();
        // turning them all off just in case
        hcanvas.lateChild.SetActive(false);
        hcanvas.perfectChild.SetActive(false);
        hcanvas.earlyChild.SetActive(false);

        if (beatPercentage < 0.4f)
        {
            hcanvas.lateChild.SetActive(true);
        }
        else if (beatPercentage < 0.8f)
        {
            hcanvas.earlyChild.SetActive(true);
        }
        else
        {
            hcanvas.perfectChild.SetActive(true);
        }

        Destroy(instance, popupLength);
    }

}
