using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class MenuController : StaticInstance<MenuController>
{
    [SerializeField] Button btnRank;
    [SerializeField] Button btnProfile;
    [SerializeField] Button btnGuide;
    [SerializeField] Button btnSettings;
    [SerializeField] Button btnPlay;
    [SerializeField] GameObject panelTool;
    [SerializeField] TextMeshProUGUI txtDisplayName;
    [SerializeField] TextMeshProUGUI txtDisplayRank;
    [SerializeField] TextMeshProUGUI txtDisplayScore;
    [SerializeField] Button btnLogout;
    public OverviewProfileData overviewProfileData;

    private void Start()
    {
        //btnRank.onClick.AddListener(() => ViewManager.Show<>)
        btnLogout.onClick.AddListener(() => SceneManager.LoadScene("LoginScene"));

        overviewProfileData = new OverviewProfileData
        {
            displayName = GlobalValue.playerName,
            displayRank = $"Rank: {"#1"}",
            displayScore = $"Score: {"3000"}",
        };

        LoadOverviewData();
    }

    public void LoadOverviewData()
    {
        txtDisplayName.text = overviewProfileData.displayName;
        txtDisplayRank.text = overviewProfileData.displayRank;
        txtDisplayScore.text = overviewProfileData.displayScore;
    }
}




public class OverviewProfileData
{
    public string displayName;
    public string displayRank;
    public string displayScore;
}
