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
}