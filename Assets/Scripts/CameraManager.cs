using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera BoardVCam; 
    public CinemachineVirtualCamera PlayerInventoryVCam; 

    void ResetCameraPriorities()
    {
        BoardVCam.Priority = 0;
        PlayerInventoryVCam.Priority = 0;
    }

    public void SetBoardCamera()
    {
        ResetCameraPriorities();
        BoardVCam.Priority = 10;
    }

    public void SetPlayerInventoryCamera()
    {
        ResetCameraPriorities();
        PlayerInventoryVCam.Priority = 10;
    }
}
