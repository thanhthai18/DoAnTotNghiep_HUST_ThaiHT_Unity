using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotRoomSelect : MonoBehaviour
{
    public string roomName;
    [SerializeField] UIButton selfButton;
    [SerializeField] TextMeshProUGUI txtRoomName;
    [SerializeField] TextMeshProUGUI txtSoLuong;
    [SerializeField] RectTransform kimRect;

    
    
    void Awake()
    {
        CollectViews();
        SetValueSoLuong(1);
        selfButton.onClickEvent.AddListener(OnClickButtonJoinRoom);
    }
    #region SUBSCRIBE
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    #endregion
    
    void Start()
    {
        

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
    public void SetValueSoLuong(int playerNum)
    {
        txtSoLuong.text = $"{playerNum}/4";
        SetValueKimRect(playerNum);
    }
    public void OnClickButtonJoinRoom()
    {
        NetworkManager.instance.JoinRoom(roomName);
    }

}

