using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateManager : MonoBehaviour
{
    public static SpectateManager instance;
    [SerializeField] private Camera cam;
    public List<CameraInfo> Cameras /*{ get; private set; }*/ = new List<CameraInfo>();
    public Camera LocalCamera;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public void AddCameraInfo(Camera _camera, CaptureHandler _captureHandler)
    {
        Cameras.Add(new CameraInfo(_camera, _captureHandler));
    }

    public void SetSceneView()
    {
        LocalCamera.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);
    }

    public void SetOtherView()
    {
        if (!RPCManager.Local.isCaptured) { return; }
        for (int i = 0; i < Cameras.Count; i++)
        {
            var cam = Cameras[i].Camera;
            var cH = Cameras[i].CaptureHandler;
            if(cam == null ||  cH == null) {  continue; }
            if (cH.isCaptured) { continue; }
            
            LocalCamera.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            cam.enabled = true;
            return;
        }
        SetSceneView();
    }
}

[System.Serializable]
public class CameraInfo
{
    public Camera Camera;
    public CaptureHandler CaptureHandler;

    public CameraInfo(Camera _camera, CaptureHandler _captureHandler)
    {
        Camera = _camera;
        CaptureHandler = _captureHandler;
    }
}