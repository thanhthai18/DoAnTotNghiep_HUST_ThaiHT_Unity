using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource_Run;
        [SerializeField] private AudioSource audioSource_Dash;
        [SerializeField] private AudioSource audioSource_FX;


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
            audioSource_Run.loop = true;
            audioSource_Run.clip = AudioController.Instance.GetAudioClipInCommon(AudioClipEnum.DataSound_running);
            audioSource_Dash.clip = AudioController.Instance.GetAudioClipInCommon(AudioClipEnum.DataSound_dash);

            AudioController.ActionOnMuteSound += OnMuteSound;
            OnMuteSound(GlobalValue.isMuteSound);
        }

        private void OnMuteSound(bool isMute)
        {
            audioSource_Run.mute = isMute;
            audioSource_Dash.mute = isMute;
            audioSource_FX.mute = isMute;
        }
       

        public void PlaySoundRun(bool isPlay)
        {
            if (isPlay)
            {
                if (!audioSource_Run.isPlaying)
                {
                    audioSource_Run.Play();
                }
            }
            else
            {
                if (audioSource_Run.isPlaying)
                {
                    audioSource_Run.Stop();
                }
            }
        }
        public void PlaySoundDash()
        {
            audioSource_Dash.Play();
        }
        public void PlaySoundDie()
        {
            audioSource_FX.PlayOneShot(AudioController.Instance.GetAudioClipInCommon(AudioClipEnum.DataSound_die));
            
        }

        private void OnDestroy()
        {
            AudioController.ActionOnMuteSound -= OnMuteSound;
        }
    }
}
