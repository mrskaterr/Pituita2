using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityInspector;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Foldout("Account", true)]
    [SerializeField] private GameObject LogPanel;
    [SerializeField] private TMP_InputField LogMailInput;
    [SerializeField] private TMP_InputField LogPasswordInput;
    [Foldout("Register", true)]
    [SerializeField] private GameObject RegPanel;
    [SerializeField] private TMP_InputField RegMailInput;
    [SerializeField] private TMP_InputField RegPasswordInput;
    [SerializeField] private TMP_InputField RegPasswordInput2;
    [Foldout("Set Name", true)]
    [SerializeField] private GameObject NameWindow;
    [SerializeField] private TMP_InputField NameInput;
    [Foldout("Reset Password", true)]
    [SerializeField] private GameObject ResPanel;
    [SerializeField] private TMP_InputField ResMailInput;
    [Foldout("Connect", true)]
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private TMP_Text loadingText;
    [Foldout("Error")]
    [SerializeField] private TMP_Text messageTxt;
    [Foldout("Sections", true)]
    [SerializeField] private GameObject accountSection;
    [SerializeField] private GameObject connectSection;
    [SerializeField] private GameObject matchmakingSection;
    [Space]
    [SerializeField] private GameObject sessionDetailsSection;
    [SerializeField] private GameObject roomDetailsSection;
    [Space]
    [SerializeField] private GameObject quickGameSection;
    [SerializeField] private GameObject publicRoomDetailsSection;
    [Foldout("Matchmaking", true)]
    [SerializeField] private Image preferredRoleIcon;
    [SerializeField] private Sprite[] preferredRoleSprites;
    [Foldout("Session Details", true)]
    [SerializeField] private TMP_Text playerNameTxtB;
    [SerializeField] private TMP_InputField sessionNameInput;
    [SerializeField] private TMP_Text CJBtnText;//TODO: blocking button
    [SerializeField] private GameObject createPanel;
    [SerializeField] private TMP_Dropdown mapSelection;
    [SerializeField] private TMP_Dropdown timeSelection;
    [SerializeField] private TMP_Dropdown huntersSelection;
    [SerializeField] private TMP_Dropdown hidersSelection;
    [SerializeField] private GameObject loadingIcon;
    [Foldout("Quick Game", true)]
    [SerializeField] private GameObject hunterPanel;
    [SerializeField] private GameObject hiderPanel;
    [Foldout("Room Details", true)]
    [SerializeField] private TMP_Text roomNameTxt;
    [SerializeField] private TMP_Text mapNameTxt;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private List<Button> roleButtons;
    [SerializeField] private Sprite[] roleIcons;
    //[SerializeField] private List<PlayerDataHandler> playerDataHandlers = new List<PlayerDataHandler>(); custom data class

    private const string joinTxt = "Join";
    private const string createTxt = "Create";
    private const string loadingDefaultTxt = "Connecting ...";

    #region Account

    private void SwitchPanel(WhichPanel _panel)
    {
        bool p = true;
        switch (_panel)
        {
            case WhichPanel.Login:
                LogPanel.SetActive(p);
                RegPanel.SetActive(!p);
                NameWindow.SetActive(!p);
                ResPanel.SetActive(!p);
                break;
            case WhichPanel.Register:
                LogPanel.SetActive(!p);
                RegPanel.SetActive(p);
                NameWindow.SetActive(!p);
                ResPanel.SetActive(!p);
                break;
            case WhichPanel.Name:
                LogPanel.SetActive(!p);
                RegPanel.SetActive(!p);
                NameWindow.SetActive(p);
                ResPanel.SetActive(!p);
                break;
            case WhichPanel.Reset:
                LogPanel.SetActive(!p);
                RegPanel.SetActive(!p);
                NameWindow.SetActive(!p);
                ResPanel.SetActive(p);
                break;
        }
    }
    public string GetMail(WhichPanel _panel) => _panel switch
    {
        WhichPanel.Login => LogMailInput.text,
        WhichPanel.Register => RegMailInput.text,
        WhichPanel.Reset => ResMailInput.text,
        _ => ""
    };
    public string GetPassword(WhichPanel _panel) => _panel switch
    {
        WhichPanel.Login => LogPasswordInput.text,
        WhichPanel.Register => RegPasswordInput.text,
        _ => ""
    };
    public string GetNewName()
    {
        return NameInput.text;
    }
    public bool CheckDoublePassword()
    {
        return RegPasswordInput.text == RegPasswordInput2.text;
    }
    public void SetMessage(string _message)
    {
        messageTxt.SetContent(_message);
    }
    #region SwitchPanel Methods

    public void Open_LoginPanel() { SwitchPanel(WhichPanel.Login); }
    public void Open_RegisterPanel() { SwitchPanel(WhichPanel.Register); }
    public void Open_ResetPanel() { SwitchPanel(WhichPanel.Reset); }
    public void Open_NameWindow() { SwitchPanel(WhichPanel.Name); }

    #endregion
    public enum WhichPanel { Login, Register, Reset, Name }

    #endregion
    #region Sections
    private void SwitchSection(WhichSection _section)
    {
        bool p = true;
        switch (_section)
        {
            case WhichSection.Account:
                accountSection.SetActive(p);
                connectSection.SetActive(!p);
                matchmakingSection.SetActive(!p);
                sessionDetailsSection.SetActive(!p);
                roomDetailsSection.SetActive(!p);
                quickGameSection.SetActive(!p);
                publicRoomDetailsSection.SetActive(!p);
                break;
            case WhichSection.Connect:
                accountSection.SetActive(!p);
                connectSection.SetActive(p);
                matchmakingSection.SetActive(!p);
                sessionDetailsSection.SetActive(!p);
                roomDetailsSection.SetActive(!p);
                quickGameSection.SetActive(!p);
                publicRoomDetailsSection.SetActive(!p);
                break;
            case WhichSection.Matchmaking:
                accountSection.SetActive(!p);
                connectSection.SetActive(!p);
                matchmakingSection.SetActive(p);
                sessionDetailsSection.SetActive(!p);
                roomDetailsSection.SetActive(!p);
                quickGameSection.SetActive(!p);
                publicRoomDetailsSection.SetActive(!p);
                break;
            case WhichSection.SessionDetails:
                accountSection.SetActive(!p);
                connectSection.SetActive(!p);
                matchmakingSection.SetActive(!p);
                sessionDetailsSection.SetActive(p);
                roomDetailsSection.SetActive(!p);
                quickGameSection.SetActive(!p);
                publicRoomDetailsSection.SetActive(!p);
                break;
            case WhichSection.RoomDetails:
                accountSection.SetActive(!p);
                connectSection.SetActive(!p);
                matchmakingSection.SetActive(!p);
                sessionDetailsSection.SetActive(!p);
                roomDetailsSection.SetActive(p);
                quickGameSection.SetActive(!p);
                publicRoomDetailsSection.SetActive(!p);
                break;
            case WhichSection.QuickGame:
                accountSection.SetActive(!p);
                connectSection.SetActive(!p);
                matchmakingSection.SetActive(!p);
                sessionDetailsSection.SetActive(!p);
                roomDetailsSection.SetActive(!p);
                quickGameSection.SetActive(p);
                publicRoomDetailsSection.SetActive(!p);
                break;
            case WhichSection.PublicRoomDetails:
                accountSection.SetActive(!p);
                connectSection.SetActive(!p);
                matchmakingSection.SetActive(!p);
                sessionDetailsSection.SetActive(!p);
                roomDetailsSection.SetActive(!p);
                quickGameSection.SetActive(!p);
                publicRoomDetailsSection.SetActive(p);
                break;
        }
    }
    #region SwitchSection Methods

    public void SetSection_Account() { SwitchSection(WhichSection.Account); }
    public void SetSection_Connect() { SwitchSection(WhichSection.Connect); }
    public void SetSection_Matchmaking() { SwitchSection(WhichSection.Matchmaking); }
    public void SetSection_SessionDetails() { SwitchSection(WhichSection.SessionDetails); }
    public void SetSection_RoomDetails() { SwitchSection(WhichSection.RoomDetails); }
    public void SetSection_QuickGame() { SwitchSection(WhichSection.QuickGame); }
    public void SetSection_PublicGame() { SwitchSection(WhichSection.PublicRoomDetails); }

    #endregion
    public enum WhichSection { Account, Connect, Matchmaking, SessionDetails, RoomDetails, QuickGame, PublicRoomDetails }

    #endregion
    #region Connect

    public void StartMoving()
    {
        StartCoroutine(LoadingText());
    }

    public void StopMoving()
    {
        StopCoroutine(LoadingText());
        loadingText.SetContent(loadingDefaultTxt);
    }

    private IEnumerator LoadingText()
    {
        int txtLength = loadingDefaultTxt.Length;
        string txt1 = loadingDefaultTxt;
        string txt2 = loadingDefaultTxt.Remove(txtLength - 1, 1);
        string txt3 = loadingDefaultTxt.Remove(txtLength - 2, 2);
        string txt4 = loadingDefaultTxt.Remove(txtLength - 3, 3);

        float intervalDuration = .5f;
        WaitForSeconds interval = new WaitForSeconds(intervalDuration);

        while (true)
        {
            loadingText.SetContent(txt4);
            yield return interval;
            loadingText.SetContent(txt3);
            yield return interval;
            loadingText.SetContent(txt2);
            yield return interval;
            loadingText.SetContent(txt1);
            yield return interval;
        }
    }

    #endregion
    #region Matchmaking

    public void SetPreferredIcon(int _index)
    {
        if(_index < 0 || _index >= preferredRoleSprites.Length)
        {
            Debug.LogError("Index out of range");
            return;
        }
        preferredRoleIcon.sprite = preferredRoleSprites[_index];
        SetSection_Matchmaking();
    }

    #endregion
    #region Session Details

    public void SetCreateButtonTxt(bool _p)
    {
        CJBtnText.SetContent(_p ? joinTxt : createTxt);
    }

    public string GetInputText_SessionName()
    {
        return sessionNameInput.text;
    }

    public void SetLoadingIconActive(bool _p)
    {
        loadingIcon.SetActive(_p);
    }

    public int GetSelectedMap()
    {
        return mapSelection.value;
    }

    public int GetSelectedDuration()
    {
        return timeSelection.value;
    }

    public int GetSelectedHuntersAmount()
    {
        return huntersSelection.value;
    }

    public int GetSelectedHidersAmount()
    {
        return hidersSelection.value;
    }

    #endregion
    #region Room Details

    public void SetRoomDisplayInfo(string _name, string _map, string _time)
    {
        roomNameTxt.SetContent(_name);
        mapNameTxt.SetContent(_map);
        timeTxt.SetContent(_time);
    }

    public void RefreshList()
    {
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }
        int playersAmount = PlayerHolder.GetPlayersAmount();
        for (int b = 0; b < roleButtons.Count; b++)//TODO: better ref to btns
        {
            ActiveRoleButton(b, true);
        }
        for (int i = 0; i < playersAmount; i++)
        {
            var item = Instantiate(playerListItemPrefab, playerListParent);
            var playerData = PlayerHolder.players[i];
            var playerItem = item.GetComponent<PlayerListItem>();
            playerItem.SetContent(playerData.nick, roleIcons[playerData.roleIndex]);
            playerItem.SetColor(playerData.isReady ? Color.green : Color.black);
            if(playerData.roleIndex != 0)
            {
                ActiveRoleButton(playerData.roleIndex, false);
            }
        }
    }

    private void ActiveRoleButton(int _index, bool _p)
    {
        roleButtons[_index].interactable = _p;
    }

    //Quick Game stuff

    public void SwapRolePanel(int _index)
    {
        switch (_index)
        {
            case 1:
                hunterPanel.SetActive(true);
                hiderPanel.SetActive(false);
                break;
            case 2:
                hunterPanel.SetActive(false);
                hiderPanel.SetActive(true);
                break;
        }
    }

    #endregion
    public void SetDisplayName(string _name) 
    {
        playerNameTxt.SetContent(_name);
        playerNameTxtB.SetContent(_name);
    }
}