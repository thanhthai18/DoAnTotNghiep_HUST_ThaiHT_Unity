using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelSetupKeyHost : MonoBehaviour
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
    private void OnEnable()
    {
        btnLeftMap.onClick.AddListener(OnClickLeftMap);
        btnRightMap.onClick.AddListener(OnClickRightMap);

        btnLeftTime.onClick.AddListener(OnClickLeftTime);
        btnRightTime.onClick.AddListener(OnClickRightTime);
    }
    private void OnDisable()
    {
        btnLeftMap.onClick.RemoveListener(OnClickLeftMap);
        btnRightMap.onClick.RemoveListener(OnClickRightMap);

        btnLeftTime.onClick.RemoveListener(OnClickLeftTime);
        btnRightTime.onClick.RemoveListener(OnClickRightTime);
    }
    #endregion
    
    void Start()
    {
        
    }

    public void SetMapChoose(int index)
    {
        imgMapChoose.sprite = dataMapScriptableObj.listSpriteMap[index];
    }
    public void SetTimeChoose(int index)
    {
        txtTimeChoose.text = listTimeChooseData[index].ToString() + "s";
    }

    public void OnClickLeftMap()
    {
        if(indexMap == 0)
        {
            indexMap = dataMapScriptableObj.listSpriteMap.Count - 1;
        }
        else
        {
            indexMap -= 1;
        }
        SetMapChoose(indexMap);
    }
    public void OnClickRightMap()
    {
        if (indexMap == dataMapScriptableObj.listSpriteMap.Count - 1)
        {
            indexMap = 0;
        }
        else
        {
            indexMap += 1;
        }
        SetMapChoose(indexMap);
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
        SetTimeChoose(indexTime);
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
        SetTimeChoose(indexTime);
    }




}
