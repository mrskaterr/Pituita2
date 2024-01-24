using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    [SerializeField] private GameObject modelRoot;
    [SerializeField] private Camera cam;
    [SerializeField] private AudioListener audioListener;
    [Header("Remote Check")]
    public bool isRemote;
    private const string layerName = "Local Player Model";

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            //Debug.Log("Spawned local player");
            SetLayerInChildren();
        }
        else
        {
            //Debug.Log("Spawned remote player");
            cam.enabled = false;
            audioListener.enabled = false;
        }
        //PlayerHolder.AddPlayerObject2List(gameObject);
        DontDestroyOnLoad(gameObject);

        isRemote = !Object.HasInputAuthority;
    }

    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    private void SetLayerInChildren()
    {
        modelRoot.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in modelRoot.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}