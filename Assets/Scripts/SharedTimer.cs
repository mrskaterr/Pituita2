using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using System.Text;

public class SharedTimer : NetworkBehaviour
{
    [Networked]
    public int seconds { get; set; } = 0;
    public int duration = 300;

    [SerializeField] private TMP_Text timerTxt;

    private const string separator = " : ";

    private void Update()
    {
        if (seconds >= duration)
        {
            RPCManager.Local.RPC_GameOver(RPCManager.Team.Hunters);
        }

        int time = duration - seconds;
        int min = 0;
        int sec = 0;
        StringBuilder timeString = new StringBuilder();

        min = time / 60;
        sec = time % 60;

        timeString.Append(min);
        timeString.Append(separator);
        timeString.Append(sec);
        
        timerTxt.text = timeString.ToString();
    }
}