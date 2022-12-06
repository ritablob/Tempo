using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Objects to Follow")]
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;


    private void LateUpdate()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 direction = player1.position - player2.position;
        Vector3 newDirection = Vector3.Cross(direction, Vector3.up).normalized;

        Vector3 newPosition = centerPoint + (newDirection * 5);
        newPosition.y = 5;

        gameObject.transform.position = newPosition;

        Debug.DrawLine(centerPoint, newPosition, Color.green);
        Debug.DrawLine(centerPoint, gameObject.transform.position, Color.red);
        gameObject.transform.LookAt(centerPoint);
    }

    Vector3 GetCenterPoint()
    {
        Vector3 center = Vector3.Lerp(player1.position, player2.position, 0.5f);

        return center;
    }
}
