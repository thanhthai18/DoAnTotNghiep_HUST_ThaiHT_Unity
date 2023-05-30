using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private int selfId;
        [SerializeField] private EllipseCollider2DForMap selfEllipseCollider;
        [SerializeField] private SpriteRenderer selfSpriteRenderer;
        [SerializeField] private Color selfColor;
        

        void Awake()
        {
            selfEllipseCollider = GetComponent<EllipseCollider2DForMap>();
            selfSpriteRenderer = GetComponent<SpriteRenderer>();
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
        
        }

        public void Init(MapInfo mapInfo)
        {
            selfId = mapInfo.id;
            selfSpriteRenderer.sprite = mapInfo.spriteMap;
            selfEllipseCollider.UpdateEllipseCollider(mapInfo.radiusVector.x, mapInfo.radiusVector.y, mapInfo.offsetPosition);
            selfColor = new Color(mapInfo.brightnessHandle, mapInfo.brightnessHandle, mapInfo.brightnessHandle, 1);
            selfSpriteRenderer.color = selfColor;
        }

    
    }
}
