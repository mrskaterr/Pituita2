using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityInspector;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [HideInInspector] public MissionManager missionManager;
    [HideInInspector] public ScoreManager scoreManager;
    [HideInInspector] public bool victory;

    public SharedTimer sharedTimer;

    [Tooltip("Game duration in seconds.")]
    [SerializeField] private int gameDuration = 300;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;
    public bool loadLobby;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else { instance = this; }

        missionManager = GetComponent<MissionManager>();
        scoreManager = GetComponent<ScoreManager>();
        sharedTimer.duration = gameDuration;
        
    }

    private void Start()
    {
        if(loadLobby && LoadingCanvas.instance == null)
            SceneManager.LoadScene(0);
        Invoke(nameof(EndLoading), 1);
    }

    private void EndLoading()
    {
        LoadingCanvas.SetActive(false);
        RPCManager.Local.StartSharedTimer();
    }

    public void OnEnd()
    {
        //lock movement
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        victoryScreen.SetActive(victory);
        defeatScreen.SetActive(!victory);
    }

    public void UpdateLeaderboard()
    {
        if (victory)
        {
            SendLeaderboard();
        }
    }

    private void SendLeaderboard()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Wins",
                    Value = 1
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateLeaderboard, OnError);
    }

    private void OnUpdateLeaderboard(UpdatePlayerStatisticsResult result) { }

    private void OnError(PlayFabError _error) { }

    public void LeaveGame()
    {
        Manager.Instance.lobbyManager.LeaveSession();
    }
}