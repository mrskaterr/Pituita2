using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;
using TMPro.SpriteAssetUtilities;
using UnityEngine.SocialPlatforms.Impl;

public class Ranking : MonoBehaviour
{
    private const string statisticName = "Wins";
    public GameObject rowPref;
    public Transform rowsParent;
    void OnLeaderboarAroundGet(UpdatePlayerStatisticsResult result) {Debug.Log("git3");}
    public void SendLoaderboard()
    {
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = statisticName,
                    Value=1
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboarAroundGet,OnError);
    }

    public void GetLeaderboardAroundPlayer() {
        Debug.Log("Git");
    var request = new GetLeaderboardAroundPlayerRequest {

        StatisticName = statisticName,

        MaxResultsCount = 10,
        
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request ,OnLeaderboarAroundGet,OnError);
    }
    void OnLeaderboarAroundGet(GetLeaderboardAroundPlayerResult result) {
        Debug.Log("Git2");


        foreach (var item in result.Leaderboard) {

            GameObject newGo = Instantiate(rowPref, rowsParent);

        TMP_Text [] texts = newGo.GetComponentsInChildren<TMP_Text>();

        texts[0].text = (item. Position + 1).ToString();

        texts[1].text = item.DisplayName;

        texts[2].text = item.StatValue.ToString();

        Debug.Log(string.Format("PLACE: {0} | ID: {1} | VALUE: {2}", item.Position, item. PlayFabId, item. StatValue));
        }
        rowPref.SetActive(false);
    }


    void OnError(PlayFabError error)
    {

    }
}