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

    private bool dealtDamage;

    private void OnEnable()
    {
        dealtDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !gameObject.transform.IsChildOf(other.transform) && !dealtDamage)
        {
            dealtDamage = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Transform launchPoint = players[0].transform;

            //Set which player's position should be launched away from
            if (gameObject.transform.IsChildOf(players[1].transform)) { launchPoint = players[1].transform; }

            switch (playerRef.lastBeat)
            {
                case "Early":
                    other.GetComponent<PlayerMovement>().TakeDamage(damage, hitStun, knockBack, launchPoint);
                    Debug.Log("Early");
                    break;
                case "Perfect":
                    float newDamage = damage * 1.5f;
                    Debug.Log("Perfect");
                    other.GetComponent<PlayerMovement>().TakeDamage(newDamage, hitStun, knockBack, launchPoint);
                    break;
            }
        }
    }
}
