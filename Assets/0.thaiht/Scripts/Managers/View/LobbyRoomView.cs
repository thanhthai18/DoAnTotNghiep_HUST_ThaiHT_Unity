using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyRoomView : View
{
    [SerializeField] public Button btnBack;
    [SerializeField] public Button btnStartGame; 

    public override void Initialize()
    {
        Debug.Log("lobby");
    }

    
}
