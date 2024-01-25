using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class GunMode : NetworkBehaviour
{
    [SerializeField] List<GameObject> vacuumMode;
    [SerializeField] List<GameObject> UnmorphMode;
    private bool canSwapMode = true;
    [HideInInspector] public bool isVacuumMode;
    [SerializeField] private PlayerHUD playerHUD;
    void Start()
    {
        isVacuumMode = true;
    }

    public void SwapMode()
    {
        RPC_SwapMode();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SwapMode()
    {
        if(isVacuumMode && canSwapMode)
        { 
            foreach(GameObject g in vacuumMode)g.SetActive(false);
            foreach (GameObject g in UnmorphMode) g.SetActive(true);
            isVacuumMode=false;
            playerHUD.SetCrosshair(1);
        }
        else if(!isVacuumMode && canSwapMode)
        {
            foreach (GameObject g in vacuumMode) g.SetActive(true);
            foreach (GameObject g in UnmorphMode) g.SetActive(false);
            isVacuumMode=true;;
            playerHUD.SetCrosshair(0);
        }
    }
}



