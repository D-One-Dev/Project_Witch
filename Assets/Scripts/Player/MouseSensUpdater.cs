using UnityEngine;
using Zenject;

public class MouseSensUpdater : MonoBehaviour
{
    private CameraLook _cameraLook;
    [Inject]
    public void Construct(CameraLook cameraLook)
    {
        _cameraLook = cameraLook;
    }
    public void UpdateMouseSens()
    {
        _cameraLook.UpdateMouseSens();
    }
}