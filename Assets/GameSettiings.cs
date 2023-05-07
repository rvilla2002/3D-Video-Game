using UnityEngine;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Transform playerCamera;
    private float mouseX, mouseY;

    private void Start()
    {
        // Set volume and sensitivity based on PlayerPrefs
        if (PlayerPrefs.HasKey("volume"))
        {
            audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        }

        if (PlayerPrefs.HasKey("sensitivity"))
        {
            mouseX = 0;
            mouseY = 0;
        }
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("sensitivity"))
        {
            float sensitivity = PlayerPrefs.GetFloat("sensitivity");
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, -90f, 90f);
            playerCamera.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
        }
    }
}
