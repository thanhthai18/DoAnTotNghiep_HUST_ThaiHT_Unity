using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotRoomSelect : MonoBehaviour
{
    public string roomName;
    public int maxPlayer;
    [SerializeField] UIButton selfButton;
    [SerializeField] TextMeshProUGUI txtRoomName;
    [SerializeField] TextMeshProUGUI txtSoLuong;
    [SerializeField] RectTransform kimRect;

    
    
    void Awake()
    {
        CollectViews();
        selfButton.onClickEvent.AddListener(OnClickButtonJoinRoom);
    }
    public void Initialize(string name, byte currentPlayers, byte maxPlayers)
    {
        SetValueRoomName(name);
        SetValueSoLuong(currentPlayers, maxPlayers);
    }



    public void CollectViews()
    {
        transform.GetComponentAtPath("Filled/Background Image/Name Text", out txtRoomName);
        transform.GetComponentAtPath("Filled/Background Image/Soluong Text", out txtSoLuong);
        transform.GetComponentAtPath("Filled/Background Image/Kim Image", out kimRect);
        selfButton = GetComponent<UIButton>();
    }
    public void SetValueKimRect(int playerNum)
    {
        float value = 0;
        switch (playerNum)
        {
            case 1:
                value = 45;
                break;
            case 2:
                value = 0;
                break;
            case 3:
                value = -45;
                break;
            case 4:
                value = -60;
                break;
            default:
                break;
        }

        kimRect.eulerAngles = new Vector3(0, 0, value);
    }

    public void SetValueRoomName(string _roomName)
    {
        roomName = _roomName;
        txtRoomName.text = roomName;
    }
    public void SetValueSoLuong(int playerNum, int maxPlayer)
    {
        txtSoLuong.text = $"{playerNum}/{maxPlayer}";
        SetValueKimRect(playerNum);
    }
    public void OnClickButtonJoinRoom()
    {
        NetworkManager.Instance.JoinRoom(roomName);
    }

}

