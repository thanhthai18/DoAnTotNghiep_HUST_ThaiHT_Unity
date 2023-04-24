using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyRoomController : MonoBehaviourPunCallbacks
{
    private LobbyRoomView lobbyRoomView;

    private void Awake()
    {
        lobbyRoomView = GetComponent<LobbyRoomView>();
    }

    private void Start()
    {
        
    }
}
