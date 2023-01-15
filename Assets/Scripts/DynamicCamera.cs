using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicCamera : MonoBehaviour
{
    [Header("Objects to Follow")]
    public Transform player1;
    public Transform player2;

    [Header("Other")]
    [SerializeField] AudioSource player1Music;
    [SerializeField] AudioSource player2Music;

    [Header("Camera Parameters")]
    [SerializeField] float cameraRotateSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField] float distanceModifier;
    [SerializeField] Vector2 horizontalBounds;
    [SerializeField] Vector2 forwardBounds;

    private void LateUpdate()
    {
        if (player2 != null)
        {
            //Calculate & set the close position for the camera
            Vector3 centerPoint = GetCenterPoint();
            float distance = GetDistance();
            Vector3 normalizedCenterpoint = new Vector3(centerPoint.x, transform.position.y, centerPoint.z);

            //Restrict cam movement to specified bounds
            normalizedCenterpoint.x = Mathf.Clamp(normalizedCenterpoint.x, horizontalBounds.x, horizontalBounds.y);
            normalizedCenterpoint.z = Mathf.Clamp(centerPoint.z - distanceModifier, forwardBounds.x, forwardBounds.y);


            if (distance >= distanceModifier)
            {
                float distanceGrowthModifier = (distance - distanceModifier) / 1.75f;
                normalizedCenterpoint.z = Mathf.Clamp(centerPoint.z - (distanceModifier + distanceGrowthModifier), forwardBounds.x, forwardBounds.y);
            }

            transform.position = Vector3.Lerp(transform.position, normalizedCenterpoint, cameraSpeed * Time.deltaTime);

            Vector3 lookDirection = centerPoint - gameObject.transform.position;
            lookDirection.Normalize();

            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), cameraRotateSpeed * Time.deltaTime);

            CheckCurrentPlayer();
        }
    }

    Vector3 GetCenterPoint()
    {
        Vector3 center = Vector3.Lerp(player1.position, player2.position, 0.5f);
        return center;
    }

    float GetDistance()
    {
        float dist = Vector3.Distance(player1.position, player2.position);
        return dist;
    }

    private void CheckCurrentPlayer()
    {

        if (player1.GetComponent<PlayerMovement>().HP > player2.GetComponent<PlayerMovement>().HP)
            BlendMusic(player1Music, player2Music);
        else if (player1.GetComponent<PlayerMovement>().HP < player2.GetComponent<PlayerMovement>().HP)
            BlendMusic(player2Music, player1Music);
        else
            BlendMusic(null, null);
    }

    private void BlendMusic(AudioSource newLead, AudioSource oldLead)
    {
        if (newLead != null && oldLead != null)
        {
            newLead.volume += Time.deltaTime;
            oldLead.volume -= Time.deltaTime;
        }
    }

    public void AddPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (player1 == null)
        {
            player1 = players[0].transform;
        }
        else
        {
            player2 = players[1].transform;
        }
    }
}
