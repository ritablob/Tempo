using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] PlayerMovement playerRef;

    [Header("Hit Values")]
    [SerializeField] int damage;
    [SerializeField] float hitStun;
    [SerializeField] float knockBack;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !gameObject.transform.IsChildOf(other.transform))
        {
            switch (playerRef.lastBeat)
            {
                case "Early":
                    other.GetComponent<PlayerMovement>().TakeDamage(damage, hitStun, knockBack, gameObject.transform);
                    
                    break;
                case "Perfect":
                    int newDamage = Mathf.RoundToInt(damage * 1.5f);
                    other.GetComponent<PlayerMovement>().TakeDamage(newDamage, hitStun, knockBack, gameObject.transform);
                    break;
            }
        }
    }
}
