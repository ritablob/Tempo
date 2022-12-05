using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Objects to Follow")]
    [SerializeField] Transform obj1;
    [SerializeField] Transform obj2;

    [Header("Camera Parameters")]
    [SerializeField] float maximumZoom;
    [SerializeField] float minimumZoom;
    [SerializeField] float cameraSpeed; //How fast the camera moves to follow the players
    [SerializeField] float zoomPower; //How much the camera zooms in/out depending on the distance between the players

    void Update()
    {
        Vector3 middlePoint = obj1.position + (obj2.position - obj1.position) / 2; //Get middle point to focus on
        transform.LookAt(middlePoint, Vector3.up); //Look at middle point between the two targets


        float distance = Vector3.Distance(obj1.position, obj2.position); //Get distance between two targets, calculate zoom of camera
        distance *= zoomPower;
        distance = Mathf.Clamp(distance, minimumZoom, maximumZoom);

        Vector3 newPosition = new Vector3(middlePoint.x * distance, gameObject.transform.position.y, middlePoint.z * distance); //Find new location for camera

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPosition, Time.deltaTime * cameraSpeed);

        //Debugging
        Debug.Log(distance);
        Debug.DrawLine(gameObject.transform.position, middlePoint);
    }
}
