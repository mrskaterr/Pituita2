using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class JarBlob : NetworkBehaviour, IInteractable
{
    [SerializeField] private EscapeGameLogic gameLogic;
    public void Interact(GameObject @object)
    {
        RPC_IncrementScore();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_IncrementScore()
    {
        gameLogic.score++;
    }
}