using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityInspector;

public class Manager : MonoBehaviour
{
    #region Singleton

    public static Manager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        UIManager = GetComponent<UIManager>();
        playfabLogin = GetComponent<PlayfabLogin>();
        lobbyManager = GetComponent<LobbyManagerV2>();
    }

    #endregion

    [Foldout("References", true, true)]
    public UIManager UIManager;
    public PlayfabLogin playfabLogin;
    public LobbyManagerV2 lobbyManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}