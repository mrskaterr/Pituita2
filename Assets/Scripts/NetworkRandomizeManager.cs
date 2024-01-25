using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkRandomizeManager : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnSeedChange))]
    public byte seed { get; set; }

    private static readonly (byte, byte) valueRange = (0, 100);

    private static void OnSeedChange(Changed<NetworkRandomizeManager> _changed)
    {
        Debug.Log($"New random seed generated: {_changed.Behaviour.seed}");
    }


    public void GenerateSeed()
    {
        RPC_SetSeed((byte) Random.Range(valueRange.Item1, valueRange.Item2));
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetSeed(byte _seed)
    {
        seed = _seed;
    }

    /// <summary>
    /// Get random integer in range (inclusive).
    /// </summary>
    /// <param name="_minValue">Low range</param>
    /// <param name="_maxValue">High range</param>
    /// <returns></returns>
    public int GetRandomNumber(int _minValue, int _maxValue)
    {
        return Mathf.RoundToInt(Mathf.Lerp(_minValue, _maxValue, (float) seed / (float) valueRange.Item2));
    }

    public override void Spawned()
    {
        base.Spawned();
        GenerateSeed();
        //print($"Random(seed: {seed}): {GetRandomNumber(0, 4)}");
    }
}