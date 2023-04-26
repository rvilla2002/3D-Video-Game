using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptMaster : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed = 5;
    public float distance = 5;
    public Vector2 pitchLimits = new Vector2(-20, 80);

    private float pitch = 0;
    private float yaw = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize pitch and yaw based on the current camera rotation
        Vector3 eulerAngles = transform.eulerAngles;
        pitch = eulerAngles.x;
        yaw = eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Update pitch and yaw based on mouse input
        yaw += cameraSpeed * Input.GetAxis("Mouse X");
        pitch -= cameraSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        // Calculate the new camera position and rotation based on pitch, yaw, and distance
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * new Vector3(0, 0, -distance);
        transform.position = player.transform.position + direction;
        transform.LookAt(player.transform.position);
    }
}

