using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using System.Threading.Tasks;
using Fusion.Sockets;
using System;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private NetworkRunnerHandler runnerHandler;
    [SerializeField] Fusion.Photon.Realtime.PhotonAppSettings photonAppSettings;

    [SerializeField] private PlayerRole playerRole = new PlayerRole();

    private GameMap gameMap;
    private GameTime gameTime;
    private LobbyRegion lobbyRegion;

    private DataValidation isSessionNameValid = DataValidation.Empty;

    private bool isThereMatchingLobby = false;
    private List<GameObject> playersList = new List<GameObject>();
    private List<SessionInfo> sessions = new List<SessionInfo>();

    public static PlayerRole playerRoles;

    private const string JCButton_Create = "Create";
    private const string JCButton_Join = "Join";

    public static Action<List<SessionInfo>> UpdateSessionsList;

    [Header("GUI")]
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject connect_btn;
    [SerializeField] private GameObject session_btnsParent;
    [Space]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private GameObject roomPanel;
    [Space]
    [SerializeField] private TMP_InputField sessionName;
    [SerializeField] private Button JCButton;
    [SerializeField] private TMP_Text JCButtonText;
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private Image regionImage;
    [Space][Space]
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerListItem;
    [Space]
    [SerializeField] private FlagsIcons flagsIcons;

    private const string nickname_PlayerPrefName = "nickname";

    private void Awake()
    {
        playerRoles = playerRole;

        UpdateSessionsList += UpdateSessionsInfo;
    }

    private void Start()
    {
        ToggleButtons(true);

        if (PlayerPrefs.HasKey(nickname_PlayerPrefName))
        {
            playerName.placeholder.GetComponent<TMP_Text>().text = PlayerPrefs.GetString(nickname_PlayerPrefName);
        }
    }

    public void Connect()
    {
        string nickname = playerName.text;
        if (!string.IsNullOrEmpty(nickname))//TOIMPROVE: more conditions
        {
            SetNick();
        }
        if (PlayerPrefs.HasKey(nickname_PlayerPrefName))
        {
            runnerHandler.InstantiateNetworkRunner(PlayerPrefs.GetString(nickname_PlayerPrefName));
            ToggleButtons(false);
            //var joinLobby = JoinLobby(runnerHandler.networkRunner, $"{lobbyRegion}-PH");
            var joinLobby = JoinLobby(runnerHandler.networkRunner, $"PH");
        }
    }

    public void JoinRandomSession()
    {
        var join = StartRandomSession(runnerHandler.networkRunner);
    }

    public void JoinOrCreateSession()
    {
        switch (isThereMatchingLobby)
        {
            case false:
                var create = StartHost(runnerHandler.networkRunner);
                break;
            case true:
                var join = JoinSession(runnerHandler.networkRunner);
                break;
        }
    }

    public async Task StartHost(NetworkRunner _runner/*, string _lobbyName = "MyCustomLobby"*/)
    {
        var customProps = new Dictionary<string, SessionProperty>();

        customProps["map"] = (int)gameMap;
        customProps["time"] = (int)gameTime;

        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = sessionName.text,
            GameMode = GameMode.Host,
            SessionProperties = customProps,
            //CustomLobbyName = _lobbyName,
        });

        if (result.Ok)
        {
            OpenPanel_Room();
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    public async Task JoinSession(NetworkRunner _runner)
    {
        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = sessionName.text,
            GameMode = GameMode.Client
        });
        if (result.Ok)
        {
            OpenPanel_Room();
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    public async Task StartRandomSession(NetworkRunner _runner)
    {
        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
        });

        if (result.Ok)
        {
            Debug.Log($"Joined to {_runner.SessionInfo.Name}, players: {_runner.SessionInfo.PlayerCount}");
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    private async Task JoinLobby(NetworkRunner _runner, string _lobbyName)
    {
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, _lobbyName);

        if (result.Ok)
        {
            // all good
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    private void UpdateSessionsInfo(List<SessionInfo> _sessionList)
    {
        sessions = _sessionList;
    }

    public void SearchSpecificSession()
    {
        int sessionsAmount = sessions.Count;
        if (sessionsAmount > 0)
        {
            for (int i = 0; i < sessionsAmount; i++)
            {
                if (sessions[i].Name == sessionName.text)
                {
                    isThereMatchingLobby = true;
                    return;
                }
            }
            isThereMatchingLobby = false;
        }
        else
        {
            isThereMatchingLobby = false;
        }

        //Debug.Log($"List updated: {sessions.Count}");
    }

    #region GUI

    public void SetNick()
    {
        PlayerPrefs.SetString(nickname_PlayerPrefName, playerName.text);
    }

    /// <summary>
    /// Switching between sets of buttons.
    /// </summary>
    /// <param name="_p">True - Connect Button; False - Session Buttons</param>
    public void ToggleButtons(bool _p)
    {
        connect_btn.SetActive(_p);
        session_btnsParent.SetActive(!_p);

        playerName.readOnly = !_p;
    }

    public void OpenPanel_Info() 
    {
        infoPanel.SetActive(true);
        joinPanel.SetActive(false);
        roomPanel.SetActive(false);
    }
    public void OpenPanel_Join() 
    {
        infoPanel.SetActive(false);
        joinPanel.SetActive(true);
        createPanel.SetActive(false);
        roomPanel.SetActive(false);
        OrderPanel_ConnectSession();
        playerNameTxt.text = PlayerPrefs.GetString(nickname_PlayerPrefName);
    }

    public void OpenPanel_Room()
    {
        infoPanel.SetActive(false);
        joinPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void UpdateConnectButtonText()
    {
        SearchSpecificSession();
        string txt = isThereMatchingLobby ? JCButton_Join : JCButton_Create;
        JCButtonText.text = txt;
        OrderPanel_ConnectSession();
    }

    private void CheckSessionNameInput()
    {
        string sessionNameTxt = sessionName.text;
        if (string.IsNullOrEmpty(sessionNameTxt)) { isSessionNameValid = DataValidation.Empty; return; }
        if (sessionNameTxt.Length < 3) { isSessionNameValid = DataValidation.TooShort; return; }

        isSessionNameValid = DataValidation.AllGood;
    }

    private void OrderPanel_ConnectSession()
    {
        CheckSessionNameInput();

        JCButtonText.text = JCButton_Create;

        switch (isSessionNameValid)
        {
            case DataValidation.AllGood:
                JCButton.interactable = true;
                break;
            case DataValidation.Empty:
                JCButton.interactable = false;
                break;
            case DataValidation.TooShort:
                JCButton.interactable = false;
                break;
        }

        createPanel.SetActive(!isThereMatchingLobby);
    }

    public void SetRegion(int _index)
    {
        regionImage.sprite = flagsIcons.GetSprite(_index);

        if (_index == 0)
        {
            photonAppSettings.AppSettings.FixedRegion = string.Empty;
            return;
        }

        lobbyRegion = (LobbyRegion)_index;
        photonAppSettings.AppSettings.FixedRegion = lobbyRegion.ToString();
    }

    private void CreatePlayerListItemPrefab(string _playerName)
    {
        var listItem = Instantiate(playerListItem, playerListParent);
        listItem.GetComponent<UserInfoPrefab>().SetNick(_playerName);
    }

    #endregion

    public enum GameMap : int
    {
        Restaurant,
        Farm,
        Factory
    }

    public enum GameTime : int
    {
        Five,
        Ten,
        Fifteen,
        TwentyFive
    }

    public enum LobbyRegion : int
    {
        automatic,
        asia,
        cn,
        jp,
        eu,
        sa,
        kr,
        us
    }

    [Serializable]
    private class FlagsIcons
    {
        #region variables

        [SerializeField] private Sprite defIcon;
        [Space]
        [SerializeField] private Sprite singapore;
        [SerializeField] private Sprite shanghai;
        [SerializeField] private Sprite tokyo;
        [SerializeField] private Sprite amsterdam;
        [SerializeField] private Sprite saoPaulo;
        [SerializeField] private Sprite seoul;
        [SerializeField] private Sprite washington; 

        #endregion

        public Sprite GetSprite(int _index) => _index switch
        {
            1 => singapore,
            2 => shanghai,
            3 => tokyo,
            4 => amsterdam,
            5 => saoPaulo,
            6 => seoul,
            7 => washington,
            0 or _ => defIcon,
        };
    }
}