using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Objects to Follow")]
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;

    [Header("Camera Parameters")]
    [SerializeField] float startBlendDistance;
    [SerializeField] float maxBlendDistance;
    [SerializeField] float cameraZoomGrowth;
    [SerializeField] float cameraHeightGrowth;
    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraRotateSpeed;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        //Calculate & set the close position for the camera
        Vector3 centerPoint = GetCenterPoint();
        Vector3 direction = player1.position - player2.position;
        Vector3 newDirection = Vector3.Cross(direction, Vector3.up).normalized;

        Vector3 newPosition = centerPoint + (newDirection * 5);
        newPosition.y = 3;

        //Calculate & set the wide position for the camera
        float distance = Vector3.Distance(player1.position, player2.position);

        if(distance > startBlendDistance) //If distance is less than 8, start blending positions
        {
            Vector3 directionToCenter = gameObject.transform.position - centerPoint; //Get axis to zoom out on, normalize it
            directionToCenter.Normalize();
            distance = Mathf.Clamp(distance * cameraZoomGrowth, 5, 99);
            Vector3 zoomPosition = centerPoint + (directionToCenter * distance);
            zoomPosition.y = Mathf.Clamp(distance * cameraHeightGrowth, 3, 99); //Get height of camera

            Vector3 blendedVector = GetBlendedVector(distance, newPosition, zoomPosition);

            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, blendedVector, cameraSpeed * Time.deltaTime);

            Debug.DrawRay(centerPoint, directionToCenter, Color.blue);
            Debug.DrawLine(centerPoint, zoomPosition, Color.green);
        }
        else //If the camera is close to the objects, get a close perspective.
        {
            gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, newPosition, cameraSpeed * Time.deltaTime);
        }

        Vector3 lookDirection = centerPoint - gameObject.transform.position;
        lookDirection.Normalize();

        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), cameraRotateSpeed * Time.deltaTime);

        Debug.DrawLine(player1.position, player2.position);
    }

    Vector3 GetBlendedVector(float distance, Vector3 pos1, Vector3 pos2)
    {
        float blendAmount = Mathf.Clamp(distance / maxBlendDistance, 0, 1);

        Vector3 blendedVector = Vector3.Lerp(pos1, pos2, blendAmount);

        return blendedVector;
    }

    Vector3 GetCenterPoint()
    {
        Vector3 center = Vector3.Lerp(player1.position, player2.position, 0.5f);

        return center;
    }
}
