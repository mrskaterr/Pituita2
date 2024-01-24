using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : MonoBehaviour, IInteractable 
{
    [HideInInspector] public MissionData mission;

    private const string defaultLayerName = "Default";
    private const string interactableLayerName = "Interactable";

    public void Disable()
    {
        int layerIndex = LayerMask.NameToLayer(defaultLayerName);
        gameObject.layer = layerIndex;
    }

    public void Enable()
    {
        int layerIndex = LayerMask.NameToLayer(interactableLayerName);
        gameObject.layer = layerIndex;
    }

    public void Interact(GameObject @object)
    {
        OnInteract(@object);
    }

    protected virtual void OnInteract(GameObject @object)
    {
        mission.NextStep();
    }
    public void NextTask()
    {
        mission.NextStep();
    }
}