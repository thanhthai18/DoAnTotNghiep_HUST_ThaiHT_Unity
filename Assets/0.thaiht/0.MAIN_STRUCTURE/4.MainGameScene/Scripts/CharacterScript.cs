using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class CharacterScript : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation anim;
        [SpineAnimation] public string anim_idle, anim_run, anim_skill, anim_die;
        [SpineSkin] public List<string> listSkin = new List<string>();

        [SerializeField] bool isSpine;


        protected virtual void Awake()
        {
            isSpine = false;
            anim = GetComponent<SkeletonAnimation>();
            if (anim != null)
            {
                isSpine = true;
            }
        }
        #region SUBSCRIBE
        protected virtual void OnEnable()
        {

        }
        protected virtual void OnDisable()
        {

        }
        #endregion

        protected virtual void Start()
        {
            if (isSpine)
            {
                anim.AnimationState.Complete += AnimComplete;
            }
        }

        protected virtual void AnimComplete(Spine.TrackEntry trackEntry)
        {
            //if (trackEntry.Animation.Name == anim_VaCham)
            //{
            //    PlayAnim(anim, anim_Idle, true);

            //    //if (SoundController.instance != null)
            //    //    SoundController.instance.PlayOneShot(SoundController.instance.soundFx_Minigame, SoundController.instance.nhacdinh);
            //}
        }

        protected virtual void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
        {
            if (isSpine)
            {
                anim.state.SetAnimation(0, nameAnim, loop);
            }
        }

        public virtual void AnimIdle()
        {
            if (isSpine)
            {
                if (anim.AnimationName != anim_idle)
                {
                    PlayAnim(anim, anim_idle, true);
                }
            }

        }
        public virtual void AnimRun()
        {
            if (isSpine)
            {
                if (anim.AnimationName != anim_run)
                {
                    PlayAnim(anim, anim_run, true);
                }
            }

        }
        public virtual void AnimSkill()
        {
            if (isSpine)
            {
                PlayAnim(anim, anim_skill, true);
            }

        }
        public virtual void AnimDie()
        {
            if (isSpine)
            {
                PlayAnim(anim, anim_die, false);
            }
        }


    }
}
