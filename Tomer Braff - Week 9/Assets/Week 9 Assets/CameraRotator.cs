using UnityEngine;
using System.Collections;

public class CameraRotator : MonoBehaviour
{
    public bool invertPitch = false;
    public float lookSensitivity = 10f;

    void Update ()
    {
        // Prevent the cursor from drifting off of the screen
        // NOTE: this should be handled somewhere else but its a convenient
        // location for this demonstration
        Cursor.lockState = CursorLockMode.Locked;
        
        Vector3 currentRotation = transform.localEulerAngles;
        // Update the camera yaw (left/right)
        float yaw = Input.GetAxis("Mouse X");
        currentRotation.y += yaw * lookSensitivity * Time.deltaTime;

        // Update teh camera pitch (up/down)
        float pitch = Input.GetAxis("Mouse Y");
        
        currentRotation.x += pitch * lookSensitivity * (invertPitch ? 1f : -1f) * Time.deltaTime;

        // Prevent overrotation
        if(currentRotation.x > 180f)
        {
            // Looking up
            if (currentRotation.x < 270.5f)
                currentRotation.x = 270.5f;
        }
        else
        {
            // Looking down
            if (currentRotation.x > 89.5f)
                currentRotation.x = 89.5f;
        }

        transform.localEulerAngles = currentRotation;
    }
}
