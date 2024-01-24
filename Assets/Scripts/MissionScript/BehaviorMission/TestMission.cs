using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMission : MissionObject
{
    protected override void OnInteract(GameObject @object)
    {
        base.OnInteract(@object);
        Debug.Log("Test: " + mission.title);
    }
}