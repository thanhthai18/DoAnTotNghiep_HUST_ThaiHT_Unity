using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayFabController : PersistentSingleton<PlayFabController>
{
    public static event Action<List<PlayerLeaderboardEntry>> ActionOnLoadSuccess;

    public static void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "RankScore",
                Value = playerScore
            }
        }
        }, result => OnStatisticsUpdated(result), FailureCallback);
    }

    private static void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
    }

    private static void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public static void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest();
        request.StartPosition = 0;
        request.StatisticName = "RankScore";
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnPlayFabError);
    }



    private static void OnLeaderboardSuccess(GetLeaderboardResult obj)
    {
        //foreach (var value in obj.Leaderboard)
        //{
        //    Debug.Log(value.StatValue + " " + value.DisplayName);
        //    if(value.DisplayName == MyPlayerValue.playerName)
        //    {
        //        MyPlayerValue.rankScore = value.StatValue;
        //    }
        //}
        ActionOnLoadSuccess?.Invoke(obj.Leaderboard);
    }

    private static void OnPlayFabError(PlayFabError obj)
    {
    }


}

