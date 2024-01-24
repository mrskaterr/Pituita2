using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Teleportyzer :  NetworkBehaviour, IInteractable
{
    [SerializeField] Transform TransformToTeleport;
    private GameObject lastPlayer;
    public void Interact(GameObject Object)
    {
        lastPlayer=Object;
        RPC_teleport();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_teleport()
    {
        lastPlayer.SetActive(false);
        lastPlayer.transform.position=TransformToTeleport.transform.position;
        lastPlayer.SetActive(true);
    } 
}
