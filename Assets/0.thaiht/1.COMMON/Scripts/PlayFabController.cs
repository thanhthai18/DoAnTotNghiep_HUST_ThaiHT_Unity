using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class PlayFabController : MonoBehaviour
    {

        public static void SubmitScore(int playerScore)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "HighScore",
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
    }
}
