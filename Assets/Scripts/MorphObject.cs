using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphObject : MonoBehaviour, IInteractable
{
    [SerializeField] private int index;
    public void Interact(GameObject @object)
    {
        Morph morph = @object.GetComponent<Morph>();
        if (morph != null)
        {
            morph.index = index;
        }
    }
}