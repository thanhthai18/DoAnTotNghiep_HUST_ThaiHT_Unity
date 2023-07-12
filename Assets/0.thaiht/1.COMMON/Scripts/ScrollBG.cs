using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class ScrollBG : MonoBehaviour
    {
        private RawImage rawImg;
        private void Awake()
        {
            rawImg = GetComponent<RawImage>();
        }

        private void Update()
        {
            rawImg.uvRect = new Rect(rawImg.uvRect.x + Time.smoothDeltaTime * 0.02f, rawImg.uvRect.y + Time.smoothDeltaTime * 0.05f, rawImg.uvRect.width, rawImg.uvRect.height);
        }


    }
}
