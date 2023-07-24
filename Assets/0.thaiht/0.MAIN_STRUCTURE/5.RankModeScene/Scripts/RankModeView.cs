using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace thaiht20183826
{
    public class RankModeView : View
    {
        public Button btnPlay;
        [SerializeField] public Button btnBack;
        [SerializeField] GameObject boxWaiting;
        [SerializeField] public TextMeshProUGUI txtWaiting;
        private int timeCountWaiting;
        public bool isCounting;
        Coroutine coroutineCountWaiting;

        public int TimeCountWaiting => timeCountWaiting;
        public override void Initialize()
        {
            ShowWaiting(false);
            isCounting = false;
            btnPlay.onClick.AddListener(ToggleCounting);
        }

        public void ShowWaiting(bool isShow)
        {
            boxWaiting.SetActive(isShow);
        }

         void ToggleCounting()
        {
            if (!isCounting)
            {
                ShowWaiting(true);
                coroutineCountWaiting = StartCoroutine(StartCountWaiting());
            }
            else
            {
                isCounting = false;
                ShowWaiting(false);
                StopCoroutine(coroutineCountWaiting);
            }
        }
        public IEnumerator StartCountWaiting()
        {
            timeCountWaiting = 0;
            isCounting = true;
            while (isCounting)
            {
                txtWaiting.text = $"Waiting...{timeCountWaiting}s";
                timeCountWaiting++;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
