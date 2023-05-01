using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class LobbyRoomController : MonoBehaviourPunCallbacks
{
    public static LobbyRoomController instance;

    [SerializeField] LobbyRoomView lobbyRoomView;
    public static event Action eventUpdateLobbyRoom;
    [SerializeField] SlotRoomSelect slotRoomPrefab;
    [SerializeField] List<SlotRoomSelect> listRoom = new List<SlotRoomSelect>();


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnPlayerNameUpdate();
        lobbyRoomView.btnCreateRoom.onClick.AddListener(OnClickBtnCreateRoom);
        lobbyRoomView.btnJoinRoom.onClick.AddListener(OnClickBtnJoinRoom);
    }

    public void OnClickBtnCreateRoom()
    {
        NetworkManager.instance.CreateRoom(lobbyRoomView.inputRoomName.text);
    }
    public void OnClickBtnJoinRoom()
    {
        NetworkManager.instance.JoinRoom(lobbyRoomView.inputRoomName.text);
    }

    public void OnPlayerNameUpdate()
    {
        PhotonNetwork.NickName = GlobalValue.playerName;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("da tham gia phong");
        RoomController.instance.CallSetBannerRoomView();
        ViewManager.Show<RoomView>();
        photonView.RPC(nameof(UpdateLobbyUI), RpcTarget.All);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("da tao phong");
        photonView.RPC(nameof(SpawnObjectRoomSelect), RpcTarget.All, PhotonNetwork.CurrentRoom.Name);
    }


    [PunRPC]
    public void UpdateLobbyUI()
    {
        eventUpdateLobbyRoom?.Invoke();
    }

    [PunRPC]
    public void SpawnObjectRoomSelect(string roomName)
    {
        Debug.Log("tao phong lenn tat ca");
        var slot = Instantiate(slotRoomPrefab, lobbyRoomView.contentSlot.transform);
        slot.SetValueRoomName(roomName);
        slot.transform.SetAsFirstSibling();
        listRoom.Add(slot);
    }
  
    public void CallRemoveRoomSelect(string roomName)
    {
        photonView.RPC(nameof(RemoveRoomSelect), RpcTarget.All, roomName);
    }

    [PunRPC]
    void RemoveRoomSelect(string roomName)
    {
        for (int i = 0; i < listRoom.Count; i++)
        {
            if (listRoom[i].roomName == roomName)
            {
                Destroy(listRoom[i].gameObject);
                listRoom.RemoveAt(i);
            }
        }    
    }
}

