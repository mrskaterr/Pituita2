using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureHandler : NetworkBehaviour
{
    [SerializeField] private LocalCameraHandler cameraHandler;

    private CarryGlobal carryGlobal;
    private HealthSystem healthSystem;
    private PlayerHUD HUD;

    [Networked(OnChanged = nameof(OnChangeRelease))]
    public bool isFree { get; set; } = true;

    [Networked(OnChanged = nameof(OnCapture))]
    public bool isCaptured { get; set; } = false;

    [Networked]
    public bool isCarried { get; set; } = false;

    [Header("Camera")]
    [SerializeField] private Camera cam;

    private void Start()
    {
        Invoke(nameof(Init), 2);
    }

    private void Init()
    {
        carryGlobal = GameManager.instance.GetComponent<CarryGlobal>();
        healthSystem = GetComponent<HealthSystem>();
        HUD = GetComponent<PlayerHUD>();

        if (!Object.HasStateAuthority)
        {
            cam.GetComponent<LocalCameraHandler>().enabled = false;
        }
        else
        {
            SpectateManager.instance.LocalCamera = cam;
            return;
        }

        SpectateManager.instance.AddCameraInfo(cam, this);
    }

    [Rpc]
    public void RPC_Capture()
    {
        var carry = carryGlobal.GetCarry();
        transform.parent = carry.holdCenter;
        carry.captureHandler = this;
        carry.available = false;
        transform.localPosition = Vector3.zero;
        cameraHandler.ChangePerspective(1);
        isFree = false;
        HUD.ToggleCrosshair(false);
        HUD.ToggleOnHitImage(false);
        isCarried = true;
    }

    [Rpc]
    public void RPC_PutDown()
    {
        transform.parent = null;
        isCarried = false;
        cameraHandler.ChangePerspective(-1);
    }

    public static void OnChangeRelease(Changed<CaptureHandler> _changed)//TODO: all logic here
    {
        if (_changed.Behaviour.isFree)
        {
            _changed.Behaviour.RPC_Release();
        }
    }

    [Rpc]
    public void RPC_Release()
    {
        transform.parent = null;
        isCarried = false;
        cameraHandler.ChangePerspective(-1);
        healthSystem.Restore();
        HUD.ToggleMiniGame(false);
    }

    public static void OnCapture(Changed<CaptureHandler> _changed)
    {
        if (_changed.Behaviour.isCaptured)
        {
            _changed.Behaviour.RPC_CheckAlive();
        }
    }

    [Rpc]
    public void RPC_CheckAlive()
    {
        if (Object.HasInputAuthority)
        {
            RPCManager.Local.isCaptured = isCaptured;
        }
        int tmp = PlayerHolder.GetAliveBlobsAmount();
        if (tmp > 0) { return; }
        else
        {
            RPCManager.Local.RPC_GameOver(RPCManager.Team.Hunters);
        }
    }

    public void ChangeView()
    {
        SpectateManager.instance.SetOtherView();
    }
}