using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CarryHandler : NetworkBehaviour
{
    [SerializeField] private GameObject gunParent;
    [SerializeField] private GameObject capturedBody;
    
    public Transform holdCenter;
    public CaptureHandler captureHandler;
    
    [Networked]
    public bool available { get; set; } = false;

    private NetworkAnimator animator;

    private void Awake()
    {
        animator = GetComponent<NetworkAnimator>();
    }

    [Rpc]
    public void RPC_Take()
    {
        Take();
    }

    [Rpc]
    public void RPC_Leave()
    {
        Leave();
    }

    public void Take()
    {
        animator.SetHoldAnim(true);
        gunParent.SetActive(false);
        capturedBody.SetActive(true);
        available = true;
    }

    public void Leave()
    {
        animator.SetHoldAnim(false);
        gunParent.SetActive(true);
        capturedBody.SetActive(false);
        captureHandler?.RPC_PutDown();
    }

    public void PutDown()
    {
        captureHandler.isFree = true;
    }
}