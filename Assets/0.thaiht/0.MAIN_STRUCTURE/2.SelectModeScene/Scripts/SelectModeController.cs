using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace thaiht20183826
{
    public class SelectModeController : MonoBehaviourPunCallbacks
    {
        public override void OnConnectedToMaster()
        {
            LoaderSystem.Loading(false);

            if (GlobalValue.currentModeGame == ModeGame.RoomMode)
            {
                PhotonNetwork.LoadLevel(SceneGame.RoomModeScene);
            }
            else if (GlobalValue.currentModeGame == ModeGame.RankMode)
            {
                PhotonNetwork.LoadLevel(SceneGame.RankModeScene);
            }
        }

    }
}
