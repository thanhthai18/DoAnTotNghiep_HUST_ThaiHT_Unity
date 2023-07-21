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
                GlobalValue.isMuteMusic = !GlobalValue.isMuteMusic;
                muteImg.gameObject.SetActive(GlobalValue.isMuteMusic);             
                AudioController.Instance.SetMuteMusic(GlobalValue.isMuteMusic);
            }
            else
            {
                GlobalValue.isMuteSound = !GlobalValue.isMuteSound;
                muteImg.gameObject.SetActive(GlobalValue.isMuteSound);               
                AudioController.Instance.SetMuteSound(GlobalValue.isMuteSound);
            }
        }

    }
}
