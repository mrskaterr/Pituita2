using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerHolder : MonoBehaviour
{
    public static List<RPCManager> players = new List<RPCManager>();
    public static List<RPCManager> hunters = new List<RPCManager>();
    public static List<RPCManager> blobs = new List<RPCManager>();

    public static void AddPlayer(RPCManager _player)
    {
        players.Add(_player);
        SetPlayerList();
    }

    public static void RemovePlayer(RPCManager _player)
    {
        players.Remove(_player);
        SetPlayerList();
    }

    public static void SetPlayerList()
    {
        hunters.Clear();
        blobs.Clear();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].IsHuman())
            {
                hunters.Add(players[i]);
            }
            else
            {
                blobs.Add(players[i]);
            }
        }
    }

    public static int GetPlayersAmount()
    {
        return players.Count;
    }

    public static int GetHuntersAmount()
    {
        return hunters.Count;
    }

    public static int GetBlobsAmount()
    {
        return blobs.Count;
    }

    public static int GetAliveBlobsAmount()
    {
        int amount = 0;
        for (int i = 0; i < blobs.Count; i++)
        {
            if (!blobs[i].isCaptured)
            {
                amount++;
            }
        }
        return amount;
    }
}