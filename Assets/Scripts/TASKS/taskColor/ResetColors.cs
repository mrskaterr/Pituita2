using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetColors : MonoBehaviour, IInteractable
{
    [SerializeField] TaskRemeberColor PanelLeds;
    public void Interact(GameObject @object)
    {
        GetComponent<AudioSource>().Play();
        Debug.Log("reset");
        PanelLeds.ResetColor();
    }
}
