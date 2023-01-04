using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicCamera : MonoBehaviour
{
    [Header("Objects to Follow")]
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;
    [SerializeField] TextMeshProUGUI textP1, textP2;

    [Header("Camera Parameters")]
    [SerializeField] float cameraRotateSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField] float distanceFromPlayers;
    [SerializeField] Vector2 horizontalBounds;
    [SerializeField] Vector2 forwardBounds;

    private void LateUpdate()
    {
        if (player2 != null)
        {
            //Calculate & set the close position for the camera
            Vector3 centerPoint = GetCenterPoint();
            Vector3 normalizedCenterpoint = new Vector3(centerPoint.x, transform.position.y, centerPoint.z);

            //Restrict cam movement to specified bounds
            normalizedCenterpoint.x = Mathf.Clamp(normalizedCenterpoint.x, horizontalBounds.x, horizontalBounds.y);
            normalizedCenterpoint.z = Mathf.Clamp(centerPoint.z - distanceFromPlayers, forwardBounds.x, forwardBounds.y);

            transform.position = Vector3.Lerp(transform.position, normalizedCenterpoint, cameraSpeed * Time.deltaTime);


            Vector3 lookDirection = centerPoint - gameObject.transform.position;
            lookDirection.Normalize();

            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), cameraRotateSpeed * Time.deltaTime);

            textP1.text = $"P1 - {player1.GetComponent<PlayerMovement>().HP}hp";
            textP2.text = $"P2 - {player2.GetComponent<PlayerMovement>().HP}hp";
        }
    }

    Vector3 GetCenterPoint()
    {
        Vector3 center = Vector3.Lerp(player1.position, player2.position, 0.5f);

        return center;
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
