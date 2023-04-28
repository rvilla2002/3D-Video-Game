using UnityEngine;

public class camera_script_master : MonoBehaviour
{
    public Transform target;
    public float distance = 250f;
    public float height = 250f;
    public float smoothSpeed = 1f;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(0, height, -distance);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
