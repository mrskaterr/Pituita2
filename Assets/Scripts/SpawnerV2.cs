using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerV2 : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    //public void Spawn(NetworkRunner _runner, PlayerRef _player)
    //{
    //    _runner.Spawn(playerPrefab, Vector3.one, Quaternion.identity, _player);
    //}

    public void SpawnPlayerParent(NetworkRunner _runner, PlayerRef _player, string _name)
    {
        var tmp = _runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, _player);
        tmp.name = _name;
        tmp.GetComponent<RPCManager>().owner = _player;
        _runner.SetPlayerObject(_player, tmp);
    }
}