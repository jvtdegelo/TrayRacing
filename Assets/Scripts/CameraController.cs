using Cinemachine;
using UnityEngine;

enum CameraMode
{
    Virtual,
    Fixed
}

public class CameraSwitcher : MonoBehaviour
{
    private int
        currentFixedCameraIndex = 0,
        currentVirtualCameraIndex = 0;
    public Camera[] fixedCameras;
    public CinemachineVirtualCamera[] cinemachineCameras;
    public CinemachineBrain cinemachineBrain;
    private CameraMode mode;

    void Start()
    {
        mode = CameraMode.Virtual;
        // activate initial camera (first fixed in array)
        cinemachineBrain.gameObject.SetActive(true);
        UpdateVirtualCameras();
    }

    void Update()
    {
        // switches cameras when C is pressed
        if (Input.GetKeyDown(KeyCode.C))
            SwitchFixedCamera();
        // switches cameras when V is pressed
        if (Input.GetKeyDown(KeyCode.V))
            SwitchVirtualCamera();
    }

    private void SwitchFixedCamera()
    {
        // increments if was not on the other mode
        if (mode == CameraMode.Fixed)
            currentFixedCameraIndex = (currentFixedCameraIndex + 1) % fixedCameras.Length;
        UpdateFixedCameras();
        mode = CameraMode.Fixed;
    }

    private void UpdateFixedCameras()
    {
        // enable next fixed camera
        for (int i = 0; i < fixedCameras.Length; i++)
            fixedCameras[i].gameObject.SetActive(i == currentFixedCameraIndex);
        // disables all virtual cameras
        cinemachineBrain.gameObject.SetActive(false);
        for (int i = 0; i < cinemachineCameras.Length; i++)
            cinemachineCameras[i].gameObject.SetActive(false);
    }
    private void SwitchVirtualCamera()
    {
        // increments if was not on the other mode
        if (mode == CameraMode.Virtual)
            currentVirtualCameraIndex = (currentVirtualCameraIndex + 1) % cinemachineCameras.Length;
        UpdateVirtualCameras();
        mode = CameraMode.Virtual;
    }

    private void UpdateVirtualCameras()
    {
        // disables all fixed cameras
        for (int i = 0; i < fixedCameras.Length; i++)
            fixedCameras[i].gameObject.SetActive(false);
        // enable next virtual camera
        cinemachineBrain.gameObject.SetActive(true);
        for (int i = 0; i < cinemachineCameras.Length; i++)
            cinemachineCameras[i].gameObject.SetActive(i == currentVirtualCameraIndex);
    }
}
