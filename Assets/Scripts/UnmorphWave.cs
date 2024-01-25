using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Fusion;

public class UnmorphWave : NetworkBehaviour
{
    [SerializeField] private float radius = 1;
    [SerializeField] private VisualEffect effect;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Use()
    {
        effect.Play();
        Logic();
    }

    private void Logic()
    {
        var blobs = FindObjectsOfType<Morph>();
        foreach (var blob in blobs)
        {
            if (Vector3.Distance(blob.transform.position, transform.position) <= radius)
            {
                blob.RPC_UnMorph();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}