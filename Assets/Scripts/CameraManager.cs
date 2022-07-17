using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera MenuVCam; 
    public CinemachineVirtualCamera BoardVCam; 
    public CinemachineVirtualCamera PlayerInventoryVCam;
    public CinemachineVirtualCamera BatlleCamera;

    void ResetCameraPriorities()
    {
        MenuVCam.Priority = 0;
        BoardVCam.Priority = 0;
        PlayerInventoryVCam.Priority = 0;
    }

    public void SetMenuCamera()
    {
        ResetCameraPriorities();
        MenuVCam.Priority = 10;
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

    public void SetBattleGroundCamera()
    {
        ResetCameraPriorities();
        BatlleCamera.Priority = 10;
    }
}
