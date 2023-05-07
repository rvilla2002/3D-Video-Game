using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script_master : MonoBehaviour
{
    public Transform target;
    //controls x axis
    public float distance = 250f;
    //controls y axis, leave alone its finicky
    public float height = 1000f;
    //basically camera sensitivity
    public float rotationDamping = 10f;
    //how fast it zooms in and out, should not even need it
    public float zoomSpeed = 20f;
    //smallest allowed to be left it low dont zoom in that much it doesnt work
    public float minZoomDistance = 200f;
    //largest allowed to zoom out
    public float maxZoomDistance = 2000f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;


    private float currentRotationAngle = 0f;
    private float currentHeight = 0f;

    void LateUpdate() {
        
        if(!PauseMenu.isPaused) {
            //grabs input from the mouse and zoom the camera
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);

            //grabs input from the mouse and rotate the camera
            float rotation = Input.GetAxis("Mouse X");
            currentRotationAngle += rotation * rotationDamping;
            currentRotationAngle = Mathf.Repeat(currentRotationAngle, 360f);

            //grabsinput form the mouse and tilt the camera
            float verticalRotation = -Input.GetAxis("Mouse Y");
            currentHeight = Mathf.Clamp(currentHeight + verticalRotation * rotationDamping, minVerticalAngle, maxVerticalAngle);

            // Calculate the position and rotation of the camera
            //i got this from the internet dont ask me how it works but it works
            Vector3 targetPosition = target.position - Quaternion.Euler(currentHeight, currentRotationAngle, 0f) * Vector3.forward * distance;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

            //basically updates the position of camera
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            transform.LookAt(target);
        }
        
    }

}