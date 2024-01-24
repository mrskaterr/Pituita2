using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasHolder : MonoBehaviour
{
    private static List<Camera> cameras = new List<Camera>();

    public static void AddCamera(Camera _cam)
    {
        cameras.Add(_cam);
    }

    public static Camera GetActiveCamera()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            Camera cam = cameras[i];
            if (cam == null) 
            { 
                cameras.RemoveAt(i);
                continue;
            }
            if (cam.gameObject.activeSelf)
            {
                return cam;
            }
        }
        return null;
    }
}