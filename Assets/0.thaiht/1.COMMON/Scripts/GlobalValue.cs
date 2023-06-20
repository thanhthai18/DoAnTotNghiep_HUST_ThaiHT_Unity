using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValue
{
    public static int countData;
    public static ModeGame currentModeGame;
    public static int indexCharacterTransfer;
    public const int LIFE_HEART_RANK_MODE = 5;
    public const int ELO_RANK = 50;
}

public static class MyPlayerValue
{
    public static string playerName;
    public static int rankScore;
}

public static class SceneGame
{
    public static readonly string LoginScene = "LoginScene";
    public static readonly string MainMenuScene = "MainMenuScene";
    public static readonly string SelectModeScene = "SelectModeScene";
    public static readonly string RoomModeScene = "RoomModeScene";
    public static readonly string RankModeScene = "RankModeScene";
    public static readonly string TrainingModeScene = "TrainingModeScene";
    public static readonly string MainGameScene = "MainGameScene";
}


public enum ModeGame
{
    RoomMode,
    RankMode,
    TrainingMode
}


