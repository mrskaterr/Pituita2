using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabLogin : MonoBehaviour//TODO: Guest Mode, Remember Me, Mail OR Name
{
    private UIManager UIm;
    public string playerName = null;

    public Action OnCorrectNameProvided;

    private void Awake()
    {
        UIm = GetComponent<UIManager>();
    }

    private void Start()
    {
        OnCorrectNameProvided += UIm.SetSection_Connect;
    }

    public void RegisterButtonMethod()
    {
        string pass = UIm.GetPassword(UIManager.WhichPanel.Register);
        if (pass.Length < 6)
        {
            UIm.SetMessage("Password too short.");
            return;
        }
        else if (!UIm.CheckDoublePassword())
        {
            UIm.SetMessage("Passwords do not match.");
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = UIm.GetMail(UIManager.WhichPanel.Register),
            Password = UIm.GetPassword(UIManager.WhichPanel.Register),
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void LoginButtonMethod()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = UIm.GetMail(UIManager.WhichPanel.Login),
            Password = UIm.GetPassword(UIManager.WhichPanel.Login),
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            { GetPlayerProfile = true }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void ResetPasswordButtonMethod()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = UIm.GetMail(UIManager.WhichPanel.Reset),
            TitleId = "4ACE0"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    public void UpdateUserTitleDisplayNameButtonMethod()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = UIm.GetNewName()
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult _result)
    {
        UIm.SetMessage("Registered and logged in!");
        UIm.Open_NameWindow();
    }

    private void OnError(PlayFabError _error)
    {
        UIm.SetMessage(_error.ErrorMessage);
        UIm.StopMoving(); //TOTEST: <---
        //Debug.Log(_error.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult _result)
    {
        UIm.SetMessage("Logged in!");
        if (_result.InfoResultPayload.PlayerProfile != null)
        {
            playerName = _result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        UIm.SetDisplayName(playerName);
        OnCorrectNameProvided();
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult _result)
    {
        UIm.SetMessage("Password reset mail sent!");
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult _result)
    {
        playerName = _result.DisplayName;
        UIm.SetDisplayName(_result.DisplayName);
        OnCorrectNameProvided();
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Wins",
            StartPosition = 0,
            MaxResultsCount = 3
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult _result)
    {
        for (int i = 0; i < _result.Leaderboard.Count; i++)
        {
            GetFavouriteCharacter(i, _result.Leaderboard[i].PlayFabId, _result.Leaderboard[i].DisplayName, _result.Leaderboard[i].StatValue);
        }
    }

    private void GetFavouriteCharacter(int _index, string _playFabId, string _name, int _score)
    {
        int index = 1;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _playFabId,
            Keys = null
        }, result =>
        {
            int tmp = -1;
            if (result.Data.ContainsKey("John_Games"))
            {
                if (int.Parse(result.Data["John_Games"].Value) >= tmp)
                {
                    tmp = int.Parse(result.Data["John_Games"].Value);
                    index = 1;
                }
                
            }
            if (result.Data.ContainsKey("Ted_Games"))
            {
                if (int.Parse(result.Data["Ted_Games"].Value) >= tmp)
                {
                    tmp = int.Parse(result.Data["Ted_Games"].Value);
                    index = 2;
                }
            }
            if (result.Data.ContainsKey("Zeki_Games"))
            {
                if (int.Parse(result.Data["Zeki_Games"].Value) >= tmp)
                {
                    tmp = int.Parse(result.Data["Zeki_Games"].Value);
                    index = 3;
                }
            }
            if (result.Data.ContainsKey("Todd_Games"))
            {
                if (int.Parse(result.Data["Todd_Games"].Value) >= tmp)
                {
                    tmp = int.Parse(result.Data["Todd_Games"].Value);
                    index = 4;
                }
            }
            if (result.Data.ContainsKey("Eevee_Games"))
            {
                if (int.Parse(result.Data["Eevee_Games"].Value) >= tmp)
                {
                    tmp = int.Parse(result.Data["Eevee_Games"].Value);
                    index = 5;
                }
            }
            if (result.Data.ContainsKey("Mitch_Games"))
            {
                if (int.Parse(result.Data["Mitch_Games"].Value) >= tmp)
                {
                    tmp = int.Parse(result.Data["Mitch_Games"].Value);
                    index = 6;
                }
            }
            UIm.userDatas[_index].SetData(_name, _score, index - 1);
        }, error => { print("Leaderboard error."); });
    }

    private int result = 0;
    private string roleID = "";
    public void UpdatePlayerData(int _role)
    {
        roleID = _role switch
        {
            1 => "John_Games",
            2 => "Ted_Games",
            3 => "Zeki_Games",
            4 => "Todd_Games",
            5 => "Eevee_Games",
            6 => "Mitch_Games"
        };

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    private void OnDataReceived(GetUserDataResult _result)
    {
        if (_result.Data != null && _result.Data.ContainsKey(roleID))
        {
            result = int.Parse(_result.Data[roleID].Value);
            //print(roleID + " = " + result);
        }
        else
        {
            result = 0;
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {roleID, (result+1).ToString() }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSent, OnError);
    }

    private void OnDataSent(UpdateUserDataResult _result) { }
}