using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class MapController : Singleton<MapController>
    {
        [SerializeField] private DataMapScriptableObj dataMapScriptableObj;
        [SerializeField] public Map currentMap;
        [SerializeField] int indexCurrentMap;
        [SerializeField] public List<Transform> listSpawnPlayer = new List<Transform>();
        
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
            //SetupMap(0);
            SetupMap((int)PhotonNetwork.CurrentRoom.CustomProperties["indexMap"]);
        }

        public void SetupMap(int index)
        {
            if (index >= dataMapScriptableObj.listMapInfo.Count || index < 0)
            {
                return;
            }
            indexCurrentMap = index;
            var data = dataMapScriptableObj.listMapInfo[indexCurrentMap];
            currentMap.Init(data);

        }

        public int GetNumberOfMap()
        {
            return dataMapScriptableObj.listMapInfo.Count;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                indexCurrentMap++;
                if (indexCurrentMap == dataMapScriptableObj.listMapInfo.Count)
                {
                    indexCurrentMap = 0;
                }
                SetupMap(indexCurrentMap);
            }
        }



    }
}
