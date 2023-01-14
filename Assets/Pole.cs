using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public void Plant()
    {
        transform.parent = null;
        StartCoroutine(Delay());
        GetComponent<Animator>().SetTrigger("Placed");
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 1);

        foreach (var hitCollider in col)
        {
            if (hitCollider.GetComponent<EventCommunicator>() != null)
            {
                hitCollider.GetComponent<EventCommunicator>().PickUpSpear(this.gameObject, 1);
                GetComponent<Collider>().enabled = false;
                GetComponent<Animator>().SetTrigger("Picked");
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;
    }
}
