using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_master_script : MonoBehaviour
{
    //500 feels good
    public float player_speed = 500;
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////////////
        // Converting the mouse position to a point in 3D-space
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        // Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
        float t = cam.transform.position.y / (cam.transform.position.y - point.y);
        Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, 1, t * (point.z - cam.transform.position.z) + cam.transform.position.z);
        //Rotating the object to that point
        transform.LookAt(finalPoint, Vector3.up);
        ////////////////////////////////////////////////////////////////////////////

        //transform.Translate(Vector3.forward * player_speed * Time.deltaTime);
        Rigidbody rb = GetComponent<Rigidbody>();
         if (Input.GetKey(KeyCode.A))
             transform.Translate(Vector3.left * player_speed * Time.deltaTime);
         if (Input.GetKey(KeyCode.D))
             transform.Translate(Vector3.right * player_speed * Time.deltaTime);
         if (Input.GetKey(KeyCode.W))
             transform.Translate(Vector3.forward * player_speed * Time.deltaTime);
         if (Input.GetKey(KeyCode.S))
             transform.Translate((Vector3.forward * player_speed * Time.deltaTime) * -1);

        //if(Input.GetKeyDown("space")) {
         //   GetComponent<RigidBody>().
        //}
    }
}
