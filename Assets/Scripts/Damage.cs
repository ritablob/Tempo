using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] PlayerMovement playerRef;

    [Header("Hit Values")]
    [SerializeField] float damage;
    [SerializeField] float hitStun;
    [SerializeField] float knockBack;
    [SerializeField] bool dealtDamage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !gameObject.transform.IsChildOf(other.transform) && !dealtDamage)
        {
            dealtDamage = true;

            switch (playerRef.lastBeat)
            {
                case "Early":
                    other.GetComponent<PlayerMovement>().TakeDamage(damage, hitStun, knockBack, gameObject.transform);
                    Debug.Log("Early");
                    break;
                case "Perfect":
                    float newDamage = damage * 1.5f;
                    Debug.Log("Perfect");
                    other.GetComponent<PlayerMovement>().TakeDamage(newDamage, hitStun, knockBack, gameObject.transform);
                    break;
            }
        }
    }
}
