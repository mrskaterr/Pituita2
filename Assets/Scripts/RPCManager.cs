using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RPCManager : NetworkBehaviour
{
    public static RPCManager Local;
    [Networked, Capacity(14)]
    public string nick { get; set; }
    [SerializeField] private Role role = Role.None;
    [Networked(OnChanged = nameof(OnRoleChange))]
    [HideInInspector] public int roleIndex { get; set; } = 0;
    [Networked(OnChanged = nameof(OnIsReadyChange))]
    public bool isReady { get; set; } = false;
    [SerializeField] private GameObject hunterAvatarA;
    [SerializeField] private GameObject hunterAvatarB;
    [SerializeField] private GameObject hunterAvatarC;
    [SerializeField] private GameObject blobAvatarA;
    [SerializeField] private GameObject blobAvatarB;
    [SerializeField] private GameObject blobAvatarC;
    //public static GameObject Avatar;
    public PlayerRef owner;
    public NetworkObject playerAvatar;
    private bool countdown = false;

    [Networked]
    public bool isCaptured { get; set; } = false;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            nick = Manager.Instance.playfabLogin.playerName;
        }
        PlayerHolder.AddPlayer(this);
        RPC_OnPlayerInRoom();
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        PlayerHolder.RemovePlayer(this);
    }

    private void Start()
    {
        gameObject.name = $"Parent ( {nick} )";
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnPlayerInRoom()
    {
        Manager.Instance.UIManager.RefreshList();
    }

    public static void OnIsReadyChange(Changed<RPCManager> _changed)
    {
        _changed.Behaviour.OnIsReadyChange();
    }

    public void OnIsReadyChange()
    {
        Manager.Instance.UIManager.RefreshList();
        if (Manager.Instance.lobbyManager.ArePlayersReady())
        {
            if (roleIndex == 0) { roleIndex = Manager.Instance.UIManager.GetAvailableRole(); }
            Manager.Instance.playfabLogin.UpdatePlayerData(roleIndex);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        LoadingCanvas.SetActive(true);
        var avatar = Manager.Instance.lobbyManager.SpawnPlayerAvatar();
        avatar.transform.name = nick;
        playerAvatar = avatar;
        yield return new WaitForSecondsRealtime(1);
        Manager.Instance.lobbyManager.ChangeNetworkScene(1);
        avatar.gameObject.SetActive(true);
    }

    public static void OnRoleChange(Changed<RPCManager> _changed)
    {
        _changed.Behaviour.OnRoleChange();
    }
    public void OnRoleChange() 
    {
        role = (Role)roleIndex;
        Manager.Instance.UIManager.RefreshList();
        PlayerHolder.SetPlayerList();
    }
    public GameObject PlayerAvatar()
    {
        return roleIndex switch
        {
            1 => hunterAvatarA,
            2 => hunterAvatarB,
            3 => hunterAvatarC,
            4 => blobAvatarA,
            5 => blobAvatarB,
            6 => blobAvatarC,
            _ => null
        };
    }

    public bool IsHuman()
    {
        if (roleIndex == 4 || roleIndex == 5 || roleIndex == 6)//TOIMPROVE:switch and list of available
        {
            return false;
        }
        return true;
    }
    public enum Role
    {
        None,
        HunterA,
        HunterB,
        HunterC,
        BlobA,
        BlobB,
        BlobC
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_GenerateRandomNumber()
    {
        Random.Range(0, 100);
    }

    public void StartSharedTimer()
    {
        RPC_ResetTimer();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ResetTimer()
    {
        countdown = false;
        StopAllCoroutines();
        GameManager.instance.sharedTimer.seconds = 0;
        countdown = true;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        WaitForSeconds interval = new WaitForSeconds(1);
        SharedTimer timer = GameManager.instance.sharedTimer;

        timer.seconds = 0;
        while (countdown)
        {
            yield return interval;
            timer.seconds++;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_GameOver(Team _team)
    {
        if (_team == Team.Hunters)
        {
            if (IsHuman())
            {
                GameManager.instance.victory = true;
            }
            else
            {
                GameManager.instance.victory = false;
            }
            GameManager.instance.OnEnd();
        }
        else if (_team == Team.Blobs)
        {
            if (IsHuman())
            {
                GameManager.instance.victory = false;
            }
            else
            {
                GameManager.instance.victory = true;
            }
            GameManager.instance.OnEnd();
        }
    }

    public enum Team
    {
        Hunters,
        Blobs
    }
}