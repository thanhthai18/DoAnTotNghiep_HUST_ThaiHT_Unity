using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class ItemEffectSpawnController : MonoBehaviourPunCallbacks
    {
        [SerializeField] SpeedUpItem speedUpItemPrefab;
        [SerializeField] SpeedDownItem speedDownItemPrefab;
        [SerializeField] TeleportItem teleportItemPrefab;
        private int timeSpawnBetweenItems;




        void Awake()
        {
            timeSpawnBetweenItems = GlobalValue.TIME_SPAWN_BETWEEN_ITEMS;
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
            NetworkManager.ActionOnMasterClientSwitched += SpawnItemOnMasterClientSwitched;
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(SpawnSpeedItem());
            }
        }

        private void SpawnItemOnMasterClientSwitched(Photon.Realtime.Player obj)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(SpawnSpeedItem());
            }
        }

        Vector3 PosSpawn()
        {
            float u = Random.Range(0f, 1f);
            float v = Random.Range(0f, 1f);

            float angle = u * 2 * Mathf.PI;
            float r = MapController.Instance.currentMap.GetMinRadius() * Mathf.Sqrt(v);
            float x = 0 + r * Mathf.Cos(angle);
            float y = 0 + r * Mathf.Sin(angle);

            return new Vector3(x, y, 0);
        }

        IEnumerator SpawnSpeedItem()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                int ran = Random.Range(0, 2);
                Vector3 posSpawn = PosSpawn();
                if (ran == 0)
                {
                    photonView.RPC(nameof(SpawnSpeedUp), RpcTarget.All, posSpawn);
                }
                else
                {
                    photonView.RPC(nameof(SpawnSpeedDown), RpcTarget.All, posSpawn);
                }
                
            }
        }
        [PunRPC]
        public void SpawnSpeedUp(Vector3 posSpawn)
        {
            var speedItem = Instantiate(speedUpItemPrefab, posSpawn, Quaternion.identity);
        }
        [PunRPC]
        public void SpawnSpeedDown(Vector3 posSpawn)
        {
            var speedItem = Instantiate(speedDownItemPrefab, posSpawn, Quaternion.identity);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            NetworkManager.ActionOnMasterClientSwitched -= SpawnItemOnMasterClientSwitched;
        }

    }
}
