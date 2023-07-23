using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace thaiht20183826
{
    public class SettingsView : View
    {
        [SerializeField] private Button btnClose;
        public static SettingsView instance;
        [SerializeField] Slider sliderSound;
        [SerializeField] Slider sliderMusic;
        [SerializeField] TMP_Dropdown dropDownFPS;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        public override void Initialize()
        {
            btnClose.onClick.AddListener(() =>
            {
                isActive = false;
                gameObject.SetActive(isActive);
            });

        }
        private void OnEnable()
        {
            sliderSound.onValueChanged.AddListener(SetVolumeSound);
            sliderMusic.onValueChanged.AddListener(SetVolumeMusic);
            dropDownFPS.onValueChanged.AddListener(ChangeFPS);
        }


        void SetVolumeSound(float value)
        {
            AudioController.Instance.SetVolSound(value);
        }
        void SetVolumeMusic(float value)
        {
            AudioController.Instance.SetVolMusic(value);
        }
        void ChangeFPS(int index)
        {
            string value = dropDownFPS.options[index].text;
            var fps = int.Parse(value.Replace(" FPS", ""));
            Debug.Log("FPS: " + fps);
            Application.targetFrameRate = fps;
        }

        public bool CheckIsActive()
        {
            return isActive;
        }

        private void OnDisable()
        {
            transform.DOKill();
            transform.DOScale(0, 0.5f);
            sliderSound.onValueChanged.RemoveListener(SetVolumeSound);
            sliderMusic.onValueChanged.RemoveListener(SetVolumeMusic);
            dropDownFPS.onValueChanged.RemoveListener(ChangeFPS);
        }
    }
}