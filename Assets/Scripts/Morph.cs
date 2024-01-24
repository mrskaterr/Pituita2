using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Morph : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnIndexChange))]
    public int index { get; set; } = -1;

    [SerializeField] private GameObject eyes;
    [SerializeField] private Renderer ren;
    [SerializeField] private List<GameObject> morphingObjects = new List<GameObject>();
    [SerializeField] private LocalCameraHandler cameraHandler;
    private CharacterInputHandler inputHandler;
    private PlayerHUD playerHUD;
    [SerializeField] private ParticleSystem particles;

    private void Awake()
    {
        inputHandler = GetComponent<CharacterInputHandler>();
        playerHUD = GetComponent<PlayerHUD>();
    }

    private static void OnIndexChange(Changed<Morph> _changed)
    {
        _changed.Behaviour.SetMorph();
    }

    private void SetMorph()
    {
        inputHandler.canSneak = index != -1;
        eyes.SetActive(index == -1);
        ren.enabled = index == -1;
        cameraHandler.ChangePerspective(index);
        for (int i = 0; i < morphingObjects.Count; i++)
        {
            morphingObjects[i].SetActive(index == i);
        }
        if (index != -1) { particles.Play(); }
        playerHUD.SetCrosshair((byte) (index == -1 ? 0 : 100));
    }
    [Rpc]
    public void RPC_UnMorph()
    {
        index=-1;
        playerHUD.SetCrosshair(0);
    }
}