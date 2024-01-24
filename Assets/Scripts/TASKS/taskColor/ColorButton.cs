using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [SerializeField] TaskRemeberColor PanelLeds;
    [SerializeField] int index;
    public void Interact(GameObject @object)
    {
        GetComponent<AudioSource>().Play();
        PanelLeds.AddSelectedColor(index);
        
        PanelLeds.player=@object;
    }
}
