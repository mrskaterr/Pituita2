using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class GunMode : NetworkBehaviour
{
    [SerializeField] GameObject vacuumMode;
    [SerializeField] GameObject vacuumMode2;
    [SerializeField] GameObject UnmorphMode;
    [SerializeField] GameObject UnmorphMode2;
    private bool canSwapMode = true;
    [HideInInspector]public bool isVacuumMode;
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
        if(vacuumMode.activeInHierarchy && canSwapMode)
        { 
            vacuumMode.SetActive(false);
            vacuumMode2.SetActive(false);
            UnmorphMode.SetActive(true);
            UnmorphMode2.SetActive(true);
            isVacuumMode=false;
            playerHUD.SetCrosshair(1);
        }
        else if(UnmorphMode.activeInHierarchy && canSwapMode)
        {
            UnmorphMode.SetActive(false);
            UnmorphMode2.SetActive(false);
            vacuumMode.SetActive(true);
            vacuumMode2.SetActive(true);
            isVacuumMode=true;;
            playerHUD.SetCrosshair(0);
        }
    }
}



