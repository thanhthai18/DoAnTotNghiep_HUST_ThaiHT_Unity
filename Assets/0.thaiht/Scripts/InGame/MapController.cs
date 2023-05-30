using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class MapController : MonoBehaviour
    {
        public static event Action<PlayerGamePlay> OnOutAreaMap;
        [SerializeField] private DataMapScriptableObj dataMapScriptableObj;
        [SerializeField] Map currentMap;
        [SerializeField] int indexCurrentMap;

        void Awake()
        {

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
            SetupMap(0);
        }

        public void SetupMap(int index)
        {
            if(index >= dataMapScriptableObj.listMapInfo.Count || index < 0)
            {
                return;
            }
            indexCurrentMap = index;
            var data = dataMapScriptableObj.listMapInfo[indexCurrentMap];
            currentMap.Init(data);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                indexCurrentMap++;
                if(indexCurrentMap == dataMapScriptableObj.listMapInfo.Count)
                {
                    indexCurrentMap = 0;
                }
                SetupMap(indexCurrentMap);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                OnOutAreaMap?.Invoke(collision.GetComponent<PlayerGamePlay>());
            }
        }

    }
}
