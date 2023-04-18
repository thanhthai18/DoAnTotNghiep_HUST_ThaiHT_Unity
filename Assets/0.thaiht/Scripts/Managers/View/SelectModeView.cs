using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectModeView : View
{
    [SerializeField] Button btnBack;
    [SerializeField] Button btnRankMode;
    [SerializeField] Button btnTraningMode;
    [SerializeField] Button btnRoomMode;

    public override void Initialize()
    {
        btnBack.onClick.AddListener(() =>
        {
            LoaderSystem.Loading(true);
            GlobalController.Instance.Wait(0.2f, () =>
            {
                SceneManager.LoadScene(SceneGame.MainMenuScene);
                LoaderSystem.Loading(false);
            });
        });

        btnRoomMode.onClick.AddListener(() => { OpenMode(btnRoomMode, 0); });
        btnRankMode.onClick.AddListener(() => { OpenMode(btnRankMode, 1); });
        btnTraningMode.onClick.AddListener(() => { OpenMode(btnTraningMode, 2); });

    }

    public void OpenMode(Button btn, int indexMode)
    {
        btn.AnimButton(-1);
        LoaderSystem.Loading(true);
        GlobalController.Instance.Wait(0.5f, () =>
        {
            LoaderSystem.Loading(false);
            
            switch (indexMode)
            {
                case 0:
                    OpenRoomMode();
                    break;
                case 1:
                    OpenRankMode();
                    break;
                case 2:
                    OpenTrainingMode();
                    break;
                default:
                    break;
            }
        });
    }

    public void OpenRoomMode()
    {
        Debug.Log("Open Room Mode");

        SceneManager.LoadScene(SceneGame.RoomModeScene);

    }
    public void OpenRankMode()
    {
        Debug.Log("Open Rank Mode");
        using (new LoaderSystem.Load())
        {
            SceneManager.LoadScene(SceneGame.RankModeScene);
        }
    }
    public void OpenTrainingMode()
    {
        Debug.Log("Open Training Mode");
        using (new LoaderSystem.Load())
        {
            SceneManager.LoadScene(SceneGame.TrainingModeScene);
        }
    }

}


