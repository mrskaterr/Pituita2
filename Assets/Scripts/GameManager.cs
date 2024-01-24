using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityInspector;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [HideInInspector] public MissionManager missionManager;
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
        victoryScreen.SetActive(victory);
        defeatScreen.SetActive(!victory);
    }
}