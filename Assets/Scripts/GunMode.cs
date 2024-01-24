using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class GunMode : NetworkBehaviour
{
    [SerializeField] GameObject firstMode;
    [SerializeField] GameObject secondMode;
    private bool canSwapMode = true;
    [HideInInspector]public bool fireMode;
    void Start()
    {
        fireMode = true;
    }

    public void SwapMode()
    {
        RPC_SwapMode();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SwapMode()
    {
        if(firstMode.activeInHierarchy && canSwapMode)
        { 
            firstMode.SetActive(false);
            secondMode.SetActive(true);
            fireMode=false;
        }
        else if(secondMode.activeInHierarchy && canSwapMode)
        {
            secondMode.SetActive(false);
            firstMode.SetActive(true);
            fireMode=true;;
        }
    } 
}



