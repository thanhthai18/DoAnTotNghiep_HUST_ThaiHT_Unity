using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;
    public Scene currentScene;
    public DataCharacter scriptableDataCharacter;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Debug.Log("Khoi tao GlobalController");
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        currentScene = arg0;
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

    
}
