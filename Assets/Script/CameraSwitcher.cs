using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex = 0;

    private void Start()
    {
        // Disable all cameras except the first one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Switch to the next camera on Space key press
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentCameraIndex++;
            if (currentCameraIndex >= cameras.Length)
            {
                currentCameraIndex = 0;
            }
            SwitchCamera(currentCameraIndex);
        }
    }

    private void SwitchCamera(int cameraIndex)
    {
        // Disable all cameras except the selected one
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == cameraIndex);
        }
    }
}

