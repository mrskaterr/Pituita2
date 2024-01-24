using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = CamerasHolder.GetActiveCamera();
    }

    private void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}