using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.UpdateLeaderboard();
    }
}