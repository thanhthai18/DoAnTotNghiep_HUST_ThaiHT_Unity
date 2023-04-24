using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SelectRoomController : MonoBehaviourPunCallbacks
{
    SelectRoomView selectRoomView;

    private void Awake()
    {
        selectRoomView = GetComponent<SelectRoomView>();
    }

    private void Start()
    {
        selectRoomView.btnCreateRoom.onClick.AddListener(OnClickBtnCreateRoom);
        selectRoomView.btnJoinRoom.onClick.AddListener(OnClickBtnJoinRoom);
    }

    public void OnClickBtnCreateRoom()
    {
        NetworkManager.instance.CreateRoom(selectRoomView.inputRoomName.text);
        Debug.Log("da tao phong");
    }
    public void OnClickBtnJoinRoom()
    {
        NetworkManager.instance.JoinRoom(selectRoomView.inputRoomName.text);
    }


}

