using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using System.Collections.ObjectModel;

public class PlayerNetworkEventsHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    private CharacterInputHandler inputHandler;
    public int roleIndex = 0;

    public void OnConnectedToServer(NetworkRunner runner)
    {
        return;
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        return;
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        return;
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        return;
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        return;
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        return;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (inputHandler == null && NetworkPlayer.Local != null)
        {
            inputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();
        }
        if (inputHandler != null)
        {
            input.Set(inputHandler.GetNetworkInput());
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        return;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.LocalPlayer == player)
        {
            //SessionInfo session = runner.SessionInfo;
            //var customProps = new Dictionary<string, SessionProperty>();

            //if (roleIndex == 1)
            //{
            //    customProps["huntersSlots"] = CreateProperty(session.Properties, "huntersSlots") + 1;
            //    customProps["hidersSlots"] = CreateProperty(session.Properties, "hidersSlots"); 
            //}
            //else
            //{
            //    Debug.Log(roleIndex);
            //    customProps["huntersSlots"] = CreateProperty(session.Properties, "huntersSlots");
            //    customProps["hidersSlots"] = CreateProperty(session.Properties, "hidersSlots") + 1;
            //}

            //session.UpdateCustomProperties(customProps);

            GetComponent<SpawnerV2>().SpawnPlayerParent(runner, player, $"Player {player.PlayerId}");
        }
    }

    //private int CreateProperty(ReadOnlyDictionary<string,SessionProperty> _properties, string _name)
    //{
    //    if (_properties.ContainsKey(_name))
    //    {
    //        return _properties[_name];
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player left: " + player.PlayerId);
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Manager.Instance.UIManager.RefreshList();
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            RPCManager.Local.playerAvatar.GetComponent<PlayerHUD>().DisplayInfo("Some1 left.");
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        return;
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        return;
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        return;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        LobbyManagerV2.sessions = sessionList;
        return;
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log(runner.name + " | sdr: " + shutdownReason.ToString());
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        return;
    }
}