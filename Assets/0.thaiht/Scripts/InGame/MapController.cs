using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class MapController : MonoBehaviour
    {
        public static event Action<PlayerGamePlay> OnOutAreaMap;


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
