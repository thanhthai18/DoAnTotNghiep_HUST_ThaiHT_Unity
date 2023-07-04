using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;
    public Scene currentScene;
    public DataCharacter scriptableDataCharacter;
    public static event Action ActionOnUpdatedGlobalLeaderboard;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Debug.Log("Khoi tao GlobalController");
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        PlayFabController.ActionOnLoadSuccess += SetGlobalValueLeaderboard;
    }



    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        currentScene = arg0;
    }
    void SetGlobalValueLeaderboard(List<PlayFab.ClientModels.PlayerLeaderboardEntry> obj)
    {
        GlobalValue.listPlayerLeaderBoard = obj;
        ActionOnUpdatedGlobalLeaderboard?.Invoke();
    }

    public void ReloadPreviousRoom()
    {
        Debug.Log("reload");
        SceneManager.sceneLoaded += DelayReJoinRoom;

        if (PhotonNetwork.IsMasterClient)
        {
            NetworkManager.Instance.ChangeScene(SceneGame.RoomModeScene);
        }

        //PhotonNetwork.LeaveRoom();
        //NetworkManager.ActionOnConnectedToMaster += MasterClientReJoining;

    }



    void DelayReJoinRoom(Scene arg0, LoadSceneMode arg1)
    {
        try
        {
            this.Wait(0.3f, () =>
            {
                if (arg0.name == SceneGame.RoomModeScene)
                {
                    if (RoomModeController.instance != null)
                    {
                        PhotonNetwork.CurrentRoom.IsOpen = true;
                        PhotonNetwork.CurrentRoom.IsVisible = true;
                        PlayFabController.GetLeaderboard();

                        RoomModeController.instance.HandleOnJoinedRoom();
                    }
                }
                LoaderSystem.Loading(false);
                SceneManager.sceneLoaded -= DelayReJoinRoom;
            });

        }
        catch (System.Exception e)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public int GetRankScorePlayer(string namePlayer)
    {
        foreach(var value in GlobalValue.listPlayerLeaderBoard)
        {
            if(value.DisplayName == namePlayer)
            {
                return value.StatValue;
            }
        }
        return 0;
    }

    private void OnDestroy()
    {
        PlayFabController.ActionOnLoadSuccess -= SetGlobalValueLeaderboard;
    }


}
