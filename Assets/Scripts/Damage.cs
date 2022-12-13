using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] TestMovement playerRef;

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
                    other.GetComponent<TestMovement>().TakeDamage(damage, hitStun, knockBack * 0.8f, gameObject.transform);
                    
                    break;
                case "Perfect":
                    int newDamage = Mathf.RoundToInt(damage * 1.5f);
                    other.GetComponent<TestMovement>().TakeDamage(newDamage, hitStun, knockBack, gameObject.transform);
                    break;
                case "Late":
                    other.GetComponent<TestMovement>().TakeDamage(damage, hitStun, knockBack * 0.8f, gameObject.transform);
                    break;
                default:
                    other.GetComponent<TestMovement>().TakeDamage(0, 0, 0, gameObject.transform);
                    break;
            }
        }
    }
}
