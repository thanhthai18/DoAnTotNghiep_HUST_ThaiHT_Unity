using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayModeView : View
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
            this.Wait(0.2f, () =>
            {
                LoaderSystem.Loading(false);
                gameObject.SetActive(false);
            });
        });

        btnRoomMode.onClick.AddListener(() => { OpenMode(btnRoomMode, 0); });
        btnRankMode.onClick.AddListener(() => { OpenMode(btnRankMode, 1); });
        btnTraningMode.onClick.AddListener(() => { OpenMode(btnTraningMode, 2); });

    }

    public void OpenMode(Button btn, int indexMode)
    {
        LoaderSystem.Loading(true);
        GlobalController.Instance.Wait(1.5f, () =>
        {
            LoaderSystem.Loading(false);
            btn.AnimButton(-1);
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

        SceneManager.LoadScene(SceneGame.RoomModeScene.ToString());

    }
    public void OpenRankMode()
    {
        Debug.Log("Open Rank Mode");
        using (new LoaderSystem.Load())
        {
            SceneManager.LoadScene(SceneGame.RankModeScene.ToString());
        }
    }
    public void OpenTrainingMode()
    {
        Debug.Log("Open Training Mode");
        using (new LoaderSystem.Load())
        {
            SceneManager.LoadScene(SceneGame.TrainingModeScene.ToString());
        }
    }

}


