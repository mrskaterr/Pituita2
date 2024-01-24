using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Morph : NetworkBehaviour
{
    [SerializeField] AudioClip morphAudioClip;
    
    [Networked(OnChanged = nameof(OnIndexChange))]
    public int index { get; set; } = -1;
    [SerializeField] private GameObject eyes;
    [SerializeField] private Renderer ren;
    [SerializeField] private List<GameObject> morphingObjects = new List<GameObject>();
    [SerializeField] private LocalCameraHandler cameraHandler;
    private CharacterInputHandler inputHandler;
    private AudioHandler audioHandler;
    private bool isUnMorph=true;
    private void Awake()
    {
        inputHandler = GetComponent<CharacterInputHandler>();
        audioHandler = GetComponent<AudioHandler>();
    }

    private static void OnIndexChange(Changed<Morph> _changed)
    {
        _changed.Behaviour.SetMorph();
    }

    private void SetMorph()
    {
        isUnMorph = index == -1;
        inputHandler.canSneak = !isUnMorph;
        eyes.SetActive(isUnMorph);
        ren.enabled = isUnMorph;
        cameraHandler.ChangePerspective(index);
        for (int i = 0; i < morphingObjects.Count; i++)
        {
            morphingObjects[i].SetActive(index == i);
        }
        if(!isUnMorph)
        {
            audioHandler.PlayClip(morphAudioClip);
        }
    }
    [Rpc]
    public void RPC_UnMorph()
    {
        index=-1;
    }
}