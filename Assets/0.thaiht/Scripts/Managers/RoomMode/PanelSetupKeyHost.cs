using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;


public class PanelSetupKeyHost : MonoBehaviourPunCallbacks
{
    [Header("MapChoose")]
    public int indexMap = 0;
    [SerializeField] Image imgMapChoose;
    [SerializeField] Button btnLeftMap, btnRightMap;

    [Header("TimeChoose")]
    public int indexTime = 0;
    private List<int> listTimeChooseData = new List<int>() { 30, 60, 90 };
    [SerializeField] TextMeshProUGUI txtTimeChoose;
    [SerializeField] Button btnLeftTime, btnRightTime;

    [Header("DataMap")]
    [SerializeField] DataMapScriptableObj dataMapScriptableObj;


    void Awake()
    {

    }
    #region SUBSCRIBE
    public override void OnEnable()
    {
        base.OnEnable();
        btnLeftMap.onClick.AddListener(OnClickLeftMap);
        btnRightMap.onClick.AddListener(OnClickRightMap);

        btnLeftTime.onClick.AddListener(OnClickLeftTime);
        btnRightTime.onClick.AddListener(OnClickRightTime);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        btnLeftMap.onClick.RemoveListener(OnClickLeftMap);
        btnRightMap.onClick.RemoveListener(OnClickRightMap);

        btnLeftTime.onClick.RemoveListener(OnClickLeftTime);
        btnRightTime.onClick.RemoveListener(OnClickRightTime);
    }
    #endregion

    void Start()
    {

    }

    public void ActiveButtonArrows(bool isActive)
    {
        btnLeftMap.gameObject.SetActive(isActive);
        btnRightMap.gameObject.SetActive(isActive);
        btnLeftTime.gameObject.SetActive(isActive);
        btnRightTime.gameObject.SetActive(isActive);
    }

    public override void OnLeftRoom()
    {
        SetMapChoose(0);
        SetTimeChoose(0);
    }

    [PunRPC]
    public void SetMapChoose(int index)
    {
        imgMapChoose.sprite = dataMapScriptableObj.listMapInfo[index].spriteMap;
    }
    [PunRPC]
    public void SetTimeChoose(int index)
    {
        txtTimeChoose.text = listTimeChooseData[index].ToString() + "s";
    }

    public void OnClickLeftMap()
    {
        if (indexMap == 0)
        {
            indexMap = dataMapScriptableObj.listMapInfo.Count - 1;
        }
        else
        {
            indexMap -= 1;
        }
        //SetMapChoose(indexMap);
        photonView.RPC(nameof(SetMapChoose), RpcTarget.AllBufferedViaServer, indexMap);
    }
    public void OnClickRightMap()
    {
        if (indexMap == dataMapScriptableObj.listMapInfo.Count - 1)
        {
            indexMap = 0;
        }
        else
        {
            indexMap += 1;
        }
        //SetMapChoose(indexMap);
        photonView.RPC(nameof(SetMapChoose), RpcTarget.AllBufferedViaServer, indexMap);
    }

    public void OnClickLeftTime()
    {
        if (indexTime == 0)
        {
            indexTime = listTimeChooseData.Count - 1;
        }
        else
        {
            indexTime -= 1;
        }
        //SetTimeChoose(indexTime);
        photonView.RPC(nameof(SetTimeChoose), RpcTarget.AllBufferedViaServer, indexTime);
    }
    public void OnClickRightTime()
    {
        if (indexTime == listTimeChooseData.Count - 1)
        {
            indexTime = 0;
        }
        else
        {
            indexTime += 1;
        }
        //SetTimeChoose(indexTime);
        photonView.RPC(nameof(SetTimeChoose), RpcTarget.AllBufferedViaServer, indexTime);
    }




}
