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
        btnRank.onClick.AddListener(() =>
        {
            if(RankView.instance != null)
            {
                if (!RankView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<RankView>(); 
                }
            }
            else
            {
                ViewManager.ShowWithAnim<RankView>();
            }
        });
        btnProfile.onClick.AddListener(() =>
        {
            if (ProfileView.instance != null)
            {
                if (!ProfileView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<ProfileView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<ProfileView>();
            }
        });
        btnGuide.onClick.AddListener(() =>
        {
            if (GuideView.instance != null)
            {
                if (!GuideView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<GuideView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<GuideView>();
            }
        });
        btnSettings.onClick.AddListener(() =>
        {
            if (SettingsView.instance != null)
            {
                if (!SettingsView.instance.CheckIsActive())
                {
                    ViewManager.ShowWithAnim<SettingsView>();
                }
            }
            else
            {
                ViewManager.ShowWithAnim<SettingsView>();
            }
        });

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
