using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtility : MonoBehaviour
{
    [SerializeField] private GameObject cameraParent;

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Destroy(cameraParent);
    }
}