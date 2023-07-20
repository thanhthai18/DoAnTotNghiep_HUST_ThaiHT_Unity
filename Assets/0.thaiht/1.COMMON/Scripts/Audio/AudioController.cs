using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace thaiht20183826
{
    public class AudioController : Singleton<AudioController>
    {
        public AudioSource sourceMusic;
        public List<AudioSource> listSourceSound = new List<AudioSource>();
        public List<AudioSource> listSourceSoundLoop = new List<AudioSource>();

        public DataSoundSciptableObj dataCommom;

        private DataSoundSciptableObj dataCurrentSound;

        private float VolumeSound
        {
            get
            {
                return PlayerPrefs.GetFloat("VolumeSound", 1);
            }
            set
            {
                PlayerPrefs.SetFloat("VolumeSound", value);
                listSourceSound.ForEach(s => s.volume = value);
                listSourceSoundLoop.ForEach(s => s.volume = value);
            }
        }

        private float VolumeMusic
        {
            get
            {
                return PlayerPrefs.GetFloat("VolumeMusic", 1 * 0.4f);
            }
            set
            {
                PlayerPrefs.SetFloat("VolumeMusic", value);
                sourceMusic.volume = value * 0.4f;
            }
        }

        private void Start()
        {
            SetVolSound(VolumeSound);
            SetVolMusic(VolumeMusic);
            PlayBackgroundMusicCommon();
            GlobalValue.isMuteSound = listSourceSound[0].mute;
            GlobalValue.isMuteMusic = sourceMusic.mute;
        }

        public float GetVolSound()
        {
            return VolumeSound;
        }

        public float GetVolMusic()
        {
            return VolumeMusic;
        }

        public void SetVolSound(float vol)
        {
            VolumeSound = vol;
        }
        public void SetVolMusic(float vol)
        {
            VolumeMusic = vol;
        }

        void SetupVolume()
        {
            listSourceSound.ForEach(s => s.volume = 1);
            listSourceSoundLoop.ForEach(s => s.volume = 1);
            sourceMusic.volume = 0.2f;
        }

        public AudioSource GetAudioSourceFXReady()
        {
            var source = listSourceSound.First(s => !s.isPlaying);
            if (source == null)
            {
                source = listSourceSound[0];
            }
            return source;
        }

        public void PlaySoundCommom(AudioClipEnum type)
        {
            GetAudioSourceFXReady().PlayOneShot(dataCommom.FindClip(type));
        }

        public void PlaySoundCommom(AudioClipEnum type, float delay)
        {
            this.Wait(delay, () =>
            {
                {
                    GetAudioSourceFXReady().PlayOneShot(dataCommom.FindClip(type));
                }
            });
        }
        public void PlayBackgroundMusicCommon()
        {
            sourceMusic.Stop();
            sourceMusic.clip = dataCommom.FindClip(AudioClipEnum.DataSound_BGMusicCommon);
            sourceMusic.Play();
        }
        public void StopBackgroundMusic()
        {
            sourceMusic.Stop();
        }
        public void PauseBackgroundMusic()
        {
            sourceMusic.Pause();
        }
        public void ResumeBackgroundMusic()
        {
            sourceMusic.UnPause();
        }
        public void PlayBackgroundMusicMinigame(string scene = null)
        {
            var temp = scene;
            if (string.IsNullOrEmpty(temp))
            {
                temp = SceneManager.GetActiveScene().name;
            }
            if (dataCurrentSound == null || dataCurrentSound.name != temp)
                dataCurrentSound = Resources.Load<DataSoundSciptableObj>("Sound/" + temp);
            sourceMusic.clip = dataCurrentSound.FindClip(AudioClipEnum.Minigame_BGMusic);
            sourceMusic.Play();
        }

        public void SetMuteMusic(bool isMute)
        {
            sourceMusic.mute = isMute;
        }
        public void SetMuteSound(bool isMute)
        {
            listSourceSound.ForEach(s => s.mute = isMute);
        }


        //public void PlaySound(AudioClipEnum type, string scene = null, bool loop = false)
        //{
        //    //  Debug.LogError("here3");
        //    var temp = scene;
        //    if (string.IsNullOrEmpty(temp))
        //    {
        //        temp = SceneManager.GetActiveScene().name;
        //    }
        //    // Debug.LogError("temp="+temp );
        //    if (dataCurrentSound == null || dataCurrentSound.name != temp)
        //        dataCurrentSound = Resources.Load<DataSoundSciptableObj>("Sound/" + temp);
        //    if (!loop)
        //    {
        //        sourceSound.PlayOneShot(dataCurrentSound.FindClip(type));
        //    }
        //    else
        //    {
        //        sourceSoundLoop.clip = dataCurrentSound.FindClip(type);
        //        sourceSoundLoop.loop = true;
        //        sourceSoundLoop.Play();
        //    }
        //}

        //public void StopSoundLoop()
        //{
        //    if (sourceSoundLoop.isPlaying)
        //    {
        //        sourceSoundLoop.Stop();
        //    }
        //    StopAllCoroutines();
        //    CancelInvoke();
        //}

        //public void PlaySoundOneTime(AudioClipEnum type, string scene = null, bool isReset = false)
        //{
        //    var temp = scene;
        //    if (string.IsNullOrEmpty(temp))
        //    {
        //        temp = SceneManager.GetActiveScene().name;
        //    }

        //    if (dataCurrentSound == null || dataCurrentSound.name != temp)
        //        dataCurrentSound = Resources.Load<DataSoundSciptableObj>("Sound/" + temp);

        //    if (isReset || !sourceSound.isPlaying)
        //    {
        //        sourceSound.clip = dataCurrentSound.FindClip(type);
        //        sourceSound.Play();
        //    }
        //}

        //public void StopSound()
        //{
        //    if (sourceSound.isPlaying)
        //    {
        //        sourceSound.Stop();
        //    }
        //}

        //public void PlayMusicGamePlay(float vol, string nameClip)
        //{
        //    var clip = Resources.Load<AudioClip>("Sound/" + "MiniGame" + "/" + nameClip);
        //    sourceMusic.volume = vol;
        //    sourceMusic.clip = clip;
        //    sourceMusic.loop = true;
        //    sourceMusic.Play();
        //}

        //public void PlayMusic(AudioClipEnum type, string scene = null)
        //{
        //    var temp = scene;
        //    if (string.IsNullOrEmpty(temp))
        //    {
        //        temp = SceneManager.GetActiveScene().name;
        //    }

        //    if (dataCurrentSound == null || dataCurrentSound.name != temp)
        //        dataCurrentSound = Resources.Load<DataSoundSciptableObj>("Sound/" + temp);
        //    sourceMusic.DOKill();
        //    sourceMusic.DOFade(0, 1).OnComplete(() =>
        //    {
        //        sourceMusic.clip = dataCurrentSound.FindClip(type);
        //        sourceMusic.Play();
        //        sourceMusic.DOFade(1, 1);
        //    });

        //}


        //private bool isUpdating;

        //public void PlaySoundInUpdate(AudioClipEnum type)
        //{
        //    if (isUpdating) return;
        //    isUpdating = true;
        //    string temp = SceneManager.GetActiveScene().name;
        //    // Debug.LogError("temp="+temp );
        //    if (dataCurrentSound == null || dataCurrentSound.name != temp)
        //        dataCurrentSound = Resources.Load<DataSoundSciptableObj>("Sound/" + temp);
        //    var au = dataCurrentSound.FindClip(type);
        //    StartCoroutine(LoopSoundInUpdate(au));
        //}

        //IEnumerator LoopSoundInUpdate(AudioClip audio)
        //{
        //    float waitTime = audio.length;
        //    var source = listSourceSound.First(s => !s.isPlaying);
        //    if(source == null)
        //    {
        //        source = listSourceSound[0];
        //    }
        //    source.clip = audio;
        //    while (isUpdating)
        //    {
        //        sourceSound.Play();
        //        yield return new WaitForSeconds(waitTime);
        //    }
        //    sourceSound.Stop();
        //}

        //public void StopPlaySoundInUpdate()
        //{
        //    isUpdating = false;
        //}

        //private void PlayMusic(string nameScene, List<int> source, List<int> played, float vol = 1)
        //{
        //    int rand = Helpers.GetRandomNumber(source, played);
        //    var clip = Resources.Load<AudioClip>("Sound/" + nameScene + "/" + rand);
        //    sourceMusic.DOKill();
        //    sourceMusic.DOFade(0, 1).OnComplete(() =>
        //    {
        //        sourceMusic.volume = vol;
        //        sourceMusic.clip = clip;
        //        sourceMusic.loop = true;
        //        sourceMusic.Play();
        //        sourceMusic.DOFade(vol, 1);
        //    });
        //}

        public void PauseSound(bool isPause)
        {
            if (isPause)
            {
                listSourceSound.ForEach(s => s.Pause());
                listSourceSoundLoop.ForEach(s => s.Pause());
            }
            else
            {
                listSourceSound.ForEach(s => s.UnPause());
                listSourceSoundLoop.ForEach(s => s.UnPause());
            }
        }

    }
}
