using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MissionManager : NetworkBehaviour
{
    
    [HideInInspector] public List<MissionData> missions = new List<MissionData>();
    public List<RoomList> rooms = new List<RoomList>();
    [Networked] public int missionIndexA { get; set; } = -1;
    [Networked] public int missionIndexB { get; set; } = -1;
    [Networked] public int missionIndexC { get; set; } = -1;
    [Networked] public int missionIndexD { get; set; } = -1;
    [Networked] public int missionIndexE { get; set; } = -1;
    [Networked] public int roomIndexA { get; set; } = -1;
    [Networked] public int roomIndexB { get; set; } = -1;
    [Networked] public int roomIndexC { get; set; } = -1;
    [Networked] public int roomIndexD { get; set; } = -1;
    [Networked] public int roomIndexE { get; set; } = -1;
    [Networked] public int LaptopPositionIndex { get; set; }=-1;
    [SerializeField] FindingTriggerMission laptop;

    public override void Spawned()
    {
        base.Spawned();
        GenerateIndexes();
        SetMissions();
        laptop.SetPosition(RandomizeLaptopPosition());
    }

    public void GenerateIndexes()
    {
        RandomizeRooms();
        RandomizeMissions();
       
    }
    private int RandomizeLaptopPosition()
    {
        return Random.Range(0,5);
    }

    private void RandomizeRooms()
    {
        int roomsAmount = rooms.Count;
        List<int> takenIndexes = new List<int>();

        roomIndexA = Random.Range(0, roomsAmount);
        takenIndexes.Add(roomIndexA);

        while (true)
        {
            roomIndexB = Random.Range(0, roomsAmount);
            if (!takenIndexes.Contains(roomIndexB))
            {
                break;
            }
        }
        takenIndexes.Add(roomIndexB);

        while (true)
        {
            roomIndexC = Random.Range(0, roomsAmount);
            if (!takenIndexes.Contains(roomIndexC))
            {
                break;
            }
        }
        takenIndexes.Add(roomIndexC);

        while (true)
        {
            roomIndexD = Random.Range(0, roomsAmount);
            if (!takenIndexes.Contains(roomIndexD))
            {
                break;
            }
        }
        takenIndexes.Add(roomIndexD);

        while (true)
        {
            roomIndexE = Random.Range(0, roomsAmount);
            if (!takenIndexes.Contains(roomIndexE))
            {
                break;
            }
        }
    }

    private void RandomizeMissions()
    {
        missionIndexA = Random.Range(0, rooms[roomIndexA].missions.Count);
        missionIndexB = Random.Range(0, rooms[roomIndexB].missions.Count);
        missionIndexC = Random.Range(0, rooms[roomIndexC].missions.Count);
        missionIndexD = Random.Range(0, rooms[roomIndexD].missions.Count);
        missionIndexE = Random.Range(0, rooms[roomIndexE].missions.Count);
    }

    private void SetMissions()
    {
        missions.Clear();

        missions.Add(rooms[roomIndexA].missions[missionIndexA]);
        missions.Add(rooms[roomIndexB].missions[missionIndexB]);
        missions.Add(rooms[roomIndexC].missions[missionIndexC]);
        missions.Add(rooms[roomIndexD].missions[missionIndexD]);
        missions.Add(rooms[roomIndexE].missions[missionIndexE]);

        for (int i = 0; i < missions.Count; i++)
        {
            missions[i].Init();
        }

    }

    [System.Serializable]
    public sealed class RoomList
    {
        public string name = "New Room";
        public List<MissionData> missions;
    }
}