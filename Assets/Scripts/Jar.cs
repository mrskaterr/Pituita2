using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour, IInteractable
{
    [SerializeField] private CaptureHandler captureHandler;

    public void Interact(GameObject @object)
    {
        @object.GetComponent<CarryHandler>().RPC_Take();
        captureHandler.RPC_Capture();
    }
}