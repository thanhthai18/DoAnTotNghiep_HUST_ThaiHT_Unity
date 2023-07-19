using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class ButtonDash : Singleton<ButtonDash>
    {
        public UIButton btnSelf;
        public Image imgCountDown;


        public void SetFillImgCountDown(float value)
        {
            imgCountDown.fillAmount = value;
        }


    }
}
