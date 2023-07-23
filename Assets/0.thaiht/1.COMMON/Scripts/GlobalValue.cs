using Photon.Realtime;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace thaiht20183826
{
    public static class GlobalValue
    {
        public static int countData;
        public static ModeGame currentModeGame;
        public static int indexCharacterTransfer;
        public const int LIFE_HEART_RANK_MODE = 5;
        public const int ELO_RANK = 50;
        public const int TIME_TO_CONTINUE = 5;
        public static List<PlayerLeaderboardEntry> listPlayerLeaderBoard = new List<PlayerLeaderboardEntry>();
        public static int indexPosSpawnPlayerGamePlay;
        public static int TIME_PLAY_RANK_MODE = 60;
        public static int MAX_TIME_MINIGAME_SOCCER = 60;
        public static bool isMuteMusic = false;
        public static bool isMuteSound = false;
        public const int DEFAULT_FPS = 120;
        public static int TIME_SPAWN_BETWEEN_ITEMS;
    }

    public static class MyPlayerValue
    {
        public static string playerName;
    }

    public static class SceneGame
    {
        public static readonly string LoginScene = "LoginScene";
        public static readonly string MainMenuScene = "MainMenuScene";
        public static readonly string SelectModeScene = "SelectModeScene";
        public static readonly string RoomModeScene = "RoomModeScene";
        public static readonly string RankModeScene = "RankModeScene";
        public static readonly string TrainingModeScene = "TrainingModeScene_Select";
        public static readonly string TrainingScene_Mninigame0 = "TrainingScene_Mninigame0";
        public static readonly string MainGameScene = "MainGameScene";
    }
    public enum MinigameSceneEnum
    {
        None,
        TrainingScene_Minigame0,

    }


    public enum ModeGame
    {
        RoomMode,
        RankMode,
        TrainingMode
    }
}

