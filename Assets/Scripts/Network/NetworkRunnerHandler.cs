using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;

    [HideInInspector] public NetworkRunner networkRunner;

    public void InstantiateNetworkRunner(string _playerName)
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = $"NR: {_playerName}";
    }

    public void GiveRole(int _index)
    {
        networkRunner.GetComponent<PlayerNetworkEventsHandler>().roleIndex = _index;
    }
}