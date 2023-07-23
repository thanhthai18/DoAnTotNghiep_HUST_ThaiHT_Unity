using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace thaiht20183826
{
    public class EffectMouse : MonoBehaviour
    {
        //private static EffectMouse Ins;

        public ParticleSystem eff;
        public ParticleSystem effRing;

        //private void Awake()
        //{
        //   if (Ins == null)
        //   {
        //      Ins = this;
        //      DontDestroyOnLoad(gameObject);
        //   } else if (Ins != this)
        //   {
        //      Destroy(gameObject);
        //   }
        //}

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                eff.transform.position = Helpers.GetMousePosWorld(Camera.main);
                effRing.transform.position = Helpers.GetMousePosWorld(Camera.main);
                eff.Play();
                effRing.Stop();
                effRing.Play();
                AudioController.Instance?.PlaySoundCommom(AudioClipEnum.DataSound_click);
            }
            else if (Input.GetMouseButton(0))
            {
                eff.transform.position = Helpers.GetMousePosWorld(Camera.main);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                eff.Stop();
            }
        }
    }
}