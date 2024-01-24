using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityInspector;
using UnityEngine.SceneManagement;

public class LobbyManagerV2 : MonoBehaviour
{
    private NetworkRunnerHandler runnerHandler;
    private PlayfabLogin playfabLogin;
    private UIManager UIm;
    private bool isThereMatchingLobby = false;

    public static List<SessionInfo> sessions = new List<SessionInfo>();

    private GameMap gameMap = GameMap.Restaurant;
    private GameTime gameTime = GameTime.Fifteen;

    public PreferredRoleType preferredRoleType = PreferredRoleType.None;

    private const string propertyName_hidersSlots = "hidersSlots";
    private const string propertyName_huntersSlots = "huntersSlots";

    private void Awake()
    {
        runnerHandler = GetComponent<NetworkRunnerHandler>();
        playfabLogin = GetComponent<PlayfabLogin>();
        UIm = GetComponent<UIManager>();
    }

    private void Start()
    {
        playfabLogin.OnCorrectNameProvided += StartConnecting;//TODO: Add anim
    }

    public void StartConnecting()
    {
        Invoke(nameof(Connect), 1);
    }

    private void Connect()//TODO: region select
    {
        runnerHandler.InstantiateNetworkRunner(playfabLogin.playerName);
        var joinLobby = JoinLobby(runnerHandler.networkRunner, $"PH");
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

    public void QuickGame()
    {
        var create = QuickSession(runnerHandler.networkRunner);
    }

    private async Task JoinLobby(NetworkRunner _runner, string _lobbyName)
    {
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom, _lobbyName);

        if (result.Ok)
        {
            UIm.SetSection_Matchmaking();
        }
        else
        {
            //Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            UIm.SetMessage($"Failed to Start: {result.ShutdownReason}");
        }
    }

    private async Task StartHost(NetworkRunner _runner/*, string _lobbyName = "MyCustomLobby"*/)
    {
        var customProps = new Dictionary<string, SessionProperty>();

        customProps["map"] = (int)gameMap;
        customProps["time"] = (int)gameTime;
        //customProps["hunters"] = huntersAmount;
        //customProps["hiders"] = hidersAmount;

        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = UIm.GetInputText_SessionName(),
            GameMode = GameMode.Shared,//FFS
            SessionProperties = customProps,
            PlayerCount = 6,
            //CustomLobbyName = _lobbyName,
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        if (result.Ok)
        {
            UIm.SetSection_RoomDetails();
            Invoke("SetGamePropertiesNames", .1f);
            Debug.Log("Room Created");
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }//TODO: Quick Game ( predefined properties )

    private async Task JoinSession(NetworkRunner _runner)
    {
        var result = await _runner.StartGame(new StartGameArgs()
        {
            SessionName = UIm.GetInputText_SessionName(),
            GameMode = GameMode.Shared,//FFS
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        if (result.Ok)
        {
            UIm.SetSection_RoomDetails();
            Debug.Log("Joined Room");
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    private async Task QuickSession(NetworkRunner _runner)
    {
        StartGameResult result;

        if (preferredRoleType == PreferredRoleType.None)
        {
            result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                PlayerCount = 6,
                SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        else
        {
            string matchName = Search4QuickSession();
            if(matchName == string.Empty)
            {
                result = await _runner.StartGame(new StartGameArgs()
                {
                    GameMode = GameMode.Shared,
                    PlayerCount = 6,
                    SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
                });
            }
            else
            {
                result = await _runner.StartGame(new StartGameArgs()
                {
                    SessionName = matchName,
                    GameMode = GameMode.Shared,
                    PlayerCount = 6,
                    SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
                });
            }
        }

        if (result.Ok)
        {
            var properties = runnerHandler.networkRunner.SessionInfo.Properties;
            int huntersAmount = 0;
            int hidersAmount = 0;

            if (properties.ContainsKey(propertyName_huntersSlots)) { huntersAmount = properties[propertyName_huntersSlots]; }
            if (properties.ContainsKey(propertyName_hidersSlots)) { hidersAmount = properties[propertyName_hidersSlots]; }

            if (huntersAmount == 3)
            {
                runnerHandler.GiveRole(2);
                UIm.SwapRolePanel(2);
                _runner.GetComponent<PlayerNetworkEventsHandler>().roleIndex = 2;
            }
            else if (hidersAmount == 3)
            {
                runnerHandler.GiveRole(1);
                UIm.SwapRolePanel(1);
                _runner.GetComponent<PlayerNetworkEventsHandler>().roleIndex = 1;
            }
            else
            {
                int roleIndex = (int)preferredRoleType;
                if(roleIndex == 0)
                {
                    roleIndex = Random.Range(1, 3);
                    preferredRoleType = (PreferredRoleType) roleIndex;
                }
                runnerHandler.GiveRole(roleIndex);
                UIm.SwapRolePanel(roleIndex);
                _runner.GetComponent<PlayerNetworkEventsHandler>().roleIndex = roleIndex;
            }
            UIm.SetSection_PublicGame();

            SessionInfo session = _runner.SessionInfo;
            var customProps = new Dictionary<string, SessionProperty>();

            if ((int)preferredRoleType == 1)
            {
                customProps["huntersSlots"] = CreateProperty(session.Properties, "huntersSlots") + 1;
                customProps["hidersSlots"] = CreateProperty(session.Properties, "hidersSlots");
            }
            else
            {
                customProps["huntersSlots"] = CreateProperty(session.Properties, "huntersSlots");
                customProps["hidersSlots"] = CreateProperty(session.Properties, "hidersSlots") + 1;
            }

            session.UpdateCustomProperties(customProps);

            Debug.Log("Joined Room");
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    private int CreateProperty(ReadOnlyDictionary<string, SessionProperty> _properties, string _name)
    {
        if (_properties.ContainsKey(_name))
        {
            return _properties[_name];
        }
        else
        {
            return 0;
        }
    }

    private bool Search4Session(string _name)
    {
        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].Name == _name) { return true; }
        }
        return false;
    }

    private string Search4QuickSession()
    {
        List<SessionInfo> matches = new List<SessionInfo>();

        switch (preferredRoleType)
        {
            case PreferredRoleType.None:
                return string.Empty;
            case PreferredRoleType.Hunters:
                for (int i = 0; i < sessions.Count; i++)
                {
                    if (sessions[i].Properties[propertyName_huntersSlots] < 3 && sessions[i].PlayerCount < 6)
                    {
                        matches.Add(sessions[i]);
                        continue;
                    }
                }
                break;
            case PreferredRoleType.Hiders:
                for (int i = 0; i < sessions.Count; i++)
                {
                    if (sessions[i].Properties[propertyName_hidersSlots] < 3 && sessions[i].PlayerCount < 6)
                    {
                        matches.Add(sessions[i]);
                        continue;
                    }
                }
                break;
        }

        if(matches.Count == 0) { return string.Empty; }

        return matches[0].Name;//TODO: sort by playersAmount
    }

    public void ChangeNetworkScene(int _index)
    {
        runnerHandler.networkRunner.SetActiveScene(_index);
    }

    public NetworkObject SpawnPlayerAvatar()
    {
        NetworkRunner runner = runnerHandler.networkRunner;
        if(runner.LocalPlayer == RPCManager.Local.owner)
        {
            var avatar = runner.Spawn(RPCManager.Local.PlayerAvatar(), Vector3.up * 100 + Vector3.one * Random.Range(0f, 2f), Quaternion.identity, RPCManager.Local.owner);//TODO: lock gravity & movement
            //DontDestroyOnLoad(avatar);
            runner.SetPlayerObject(runner.LocalPlayer, avatar);
            return avatar;
        }
        return null;
    }

    public void SetCJButton()
    {
        isThereMatchingLobby = Search4Session(UIm.GetInputText_SessionName());
        UIm.SetCreateButtonTxt(isThereMatchingLobby);
    }

    #region SetLobbyPlayerData

    public void SetRole(int _index)
    {
        RPCManager.Local.roleIndex = _index;
    }

    public void IsReady()
    {
        RPCManager.Local.isReady = !RPCManager.Local.isReady;
    }

    public bool ArePlayersReady()
    {
        int playersAmount = PlayerHolder.GetPlayersAmount();
        for (int i = 0; i < playersAmount; i++)
        {
            var playerData = PlayerHolder.players[i];
            if (!playerData.isReady) { return false; }
        }
        return true;
    }

    //Quick game stuff

    public void SetPreferredRole(int _index)
    {
        preferredRoleType = (PreferredRoleType) _index;
    }

    #endregion
    #region Game Properties

    public void SetGameProperties()
    {
        gameMap = GetGameMap();
        gameTime = (GameTime)UIm.GetSelectedDuration();
        //huntersAmount = GetGameHuntersAmount();
        //hidersAmount = GetGameHidersAmount();
    }

    private GameMap GetGameMap() => UIm.GetSelectedMap() switch
    {
        0 => GameMap.Restaurant,
        1 => GameMap.Farm,
        2 => GameMap.Factory,
        _ => GameMap.Restaurant,
    };

    private void SetGamePropertiesNames()
    {

        SessionInfo session = runnerHandler.networkRunner.SessionInfo;
        int tmp = session.Properties["map"];
        int tmp2 = session.Properties["time"];
        string sessionName = session.Name;
        string mapName = tmp switch
        {
            0 => "Restaurant",
            1 => "Farm",
            2 => "Factory",
            _ => "Map Name",
        };
        string durationName = tmp2 switch
        {
            0 => "5 min",
            1 => "10 min",
            2 => "15 min",
            3 => "25 min",
            _ => "Duration",
        };

        UIm.SetRoomDisplayInfo(sessionName, mapName, durationName);
    }

    private GameTime GetGameTime() => UIm.GetSelectedDuration() switch
    {
        0 => GameTime.Five,
        1 => GameTime.Ten,
        2 => GameTime.Fifteen,
        3 => GameTime.TwentyFive,
        _ => GameTime.Fifteen,
    };

    private int GetGameHuntersAmount() => UIm.GetSelectedHuntersAmount() switch
    {
        0 => 1,
        1 => 2,
        2 => 3,
        _ => 2,
    };

    private int GetGameHidersAmount() => UIm.GetSelectedHidersAmount() switch
    {
        0 => 1,
        1 => 2,
        2 => 3,
        _ => 3,
    };

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

    public enum PreferredRoleType : int
    {
        None = 0,
        Hunters = 1,
        Hiders = 2
    }
    #endregion

    public void LeaveSession()
    {
        runnerHandler.networkRunner.Shutdown();
        SceneManager.LoadScene(0);
    }
}