using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SelectModeView : View
{
    [SerializeField] UIButton btnBack;
    [SerializeField] UIButton btnRankMode;
    [SerializeField] UIButton btnTraningMode;
    [SerializeField] UIButton btnRoomMode;

    public override void Initialize()
    {
        btnBack.onClickEvent.AddListener(() =>
        {
            LoaderSystem.Loading(true);
            GlobalController.Instance.Wait(0.2f, () =>
            {
                SceneManager.LoadScene(SceneGame.MainMenuScene);
                LoaderSystem.Loading(false);
            });
        });
        btnRoomMode.onClickEvent.AddListener(() => { OpenMode(0); });
        btnRankMode.onClickEvent.AddListener(() => { OpenMode(1); });
        btnTraningMode.onClickEvent.AddListener(() => { OpenMode(2); });

    }


    public void OpenMode(int indexMode)
    {
        //btn.AnimButton(-1);
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


