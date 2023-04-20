using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script_master : MonoBehaviour
{
    public GameObject player;
    public float camera_speed = 5;

    private Vector3 offset = new Vector3(0,90,200);
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for camera to follow mouse
        transform.eulerAngles += camera_speed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        
        //to stick to player
        transform.position = player.transform.position + offset;

        //new shit
        

    }
}
