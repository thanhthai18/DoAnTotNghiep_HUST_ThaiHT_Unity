using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class ToggleButtonAudio : MonoBehaviour
    {
        private Button btnSelf;
        [SerializeField] bool isMusic;
        [SerializeField] GameObject muteImg;

        void Awake()
        {
            btnSelf = GetComponent<Button>();
        }
        #region SUBSCRIBE
        private void OnEnable()
        {
            btnSelf.onClick.AddListener(OnToggle);
            if (isMusic)
            {
                muteImg.gameObject.SetActive(GlobalValue.isMuteMusic);
                AudioController.Instance.SetMuteMusic(GlobalValue.isMuteMusic);
            }
            else
            {
                muteImg.gameObject.SetActive(GlobalValue.isMuteSound);
                AudioController.Instance.SetMuteMusic(GlobalValue.isMuteSound);
            }
   
        }
        private void OnDisable()
        {
            btnSelf.onClick.RemoveListener(OnToggle);
        }
        #endregion

        void Start()
        {

        }

        public void OnToggle()
        {           
            if (isMusic)
            {
                if (GlobalValue.isMuteMusic)
                {
                    GlobalValue.isMuteMusic = false;
                    muteImg.gameObject.SetActive(false);
                }
                else
                {
                    GlobalValue.isMuteMusic = true;
                    muteImg.gameObject.SetActive(true);
                }
                AudioController.Instance.SetMuteMusic(GlobalValue.isMuteMusic);
            }
            else
            {
                if (GlobalValue.isMuteSound)
                {
                    GlobalValue.isMuteSound = false;
                    muteImg.gameObject.SetActive(false);
                }
                else
                {
                    GlobalValue.isMuteSound = true;
                    muteImg.gameObject.SetActive(true);
                }
                AudioController.Instance.SetMuteSound(GlobalValue.isMuteSound);
            }
        }

    }
}
