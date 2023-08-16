using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class PlayFabController : PersistentSingleton<PlayFabController>
    {
        public static event Action<List<PlayerLeaderboardEntry>> ActionOnLoadSuccess;
        public static event Action<List<PlayerLeaderboardEntry>> ActionOnLoadSuccessMinigameSoccer;

        private void Start()
        {

        }

        public static void SetTimeByServer()
        {
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
              OnGetTitleDataSuccess, OnGetTitleDataFailure);
        }
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
        public static void SubmitScoreMinigame(int playerScore, MinigameSceneEnum minigameName)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = minigameName.ToString(),
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
        public static void GetLeaderboardMinigame(MinigameSceneEnum minigameName)
        {
            var request = new GetLeaderboardRequest();
            request.StartPosition = 0;
            request.StatisticName = minigameName.ToString();
            switch (minigameName)
            {
                case MinigameSceneEnum.None:
                    break;
                case MinigameSceneEnum.TrainingScene_Minigame0:
                    PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess_MinigameSoccer, OnPlayFabError);
                    break;
            }
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

            ActionOnLoadSuccessMinigameSoccer?.Invoke(obj.Leaderboard);
        }
        private static void OnLeaderboardSuccess_MinigameSoccer(GetLeaderboardResult obj)
        {
            ActionOnLoadSuccessMinigameSoccer?.Invoke(obj.Leaderboard);
        }

        private static void OnPlayFabError(PlayFabError obj)
        {
        }


        #region TitleKeyData
        private static void OnGetTitleDataSuccess(GetTitleDataResult result)
        {
            if (result.Data.TryGetValue("TIME_PLAY_RANK_MODE", out string titleDataValue0))
            {
                GlobalValue.TIME_PLAY_RANK_MODE = int.Parse(titleDataValue0);
                Debug.Log("TIME_PLAY_RANK_MODE: " + GlobalValue.TIME_PLAY_RANK_MODE);
            }
            if (result.Data.TryGetValue("MAX_TIME_MINIGAME_SOCCER", out string titleDataValue1))
            {
                GlobalValue.MAX_TIME_MINIGAME_SOCCER = int.Parse(titleDataValue1);
                Debug.Log("MAX_TIME_MINIGAME_SOCCER: " + GlobalValue.MAX_TIME_MINIGAME_SOCCER);
            }

            if (result.Data.TryGetValue("TIME_SPAWN_BETWEEN_ITEMS", out string titleDataValue2))
            {
                GlobalValue.TIME_SPAWN_BETWEEN_ITEMS = int.Parse(titleDataValue2);
                Debug.Log("TIME_SPAWN_BETWEEN_ITEMS: " + GlobalValue.TIME_SPAWN_BETWEEN_ITEMS);
            }


        }

        private static void OnGetTitleDataFailure(PlayFabError error)
        {
            Debug.LogError("GetTitleData failed: " + error.ErrorMessage);
        }

        #endregion
    }
}
