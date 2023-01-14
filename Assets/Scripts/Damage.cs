using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] PlayerMovement playerRef;

    [Header("Hit Values")]
    [SerializeField] float baseDamage;
    [SerializeField] float hitStun;
    [SerializeField] float knockBack;

    private bool dealtDamage;

    private void OnEnable()
    {
        dealtDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject.GetComponent<PlayerMovement>() != playerRef && !dealtDamage)
        {
            dealtDamage = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Transform launchPoint = players[0].transform;

            //Set which player's position should be launched away from
            if (gameObject.transform.IsChildOf(players[1].transform)) { launchPoint = players[1].transform; }

            //Apply damage modifier based on timing
            float modifier = playerRef.lastBeatPercentage * (playerRef.lastBeatPercentage / 2);
            modifier *= 2.5f;
            float newDamage = baseDamage * modifier;

            Debug.Log($"{playerRef.lastBeatPercentage}%, {newDamage} damage, {modifier} modifier");

            other.GetComponent<PlayerMovement>().TakeDamage(newDamage, hitStun, knockBack, launchPoint);
        }
    }
}
