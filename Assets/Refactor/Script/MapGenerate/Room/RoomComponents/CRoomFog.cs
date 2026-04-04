using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerate
{
    internal class CRoomFog : MonoBehaviour, IRoomGeneratorComponents
    {
        [SerializeField] private Color fogColor;
        [SerializeField] private int fogLayerId;
        [SerializeField] private string sortingLayerName = "UI";
        [SerializeField] private float fogDensityRate;
        [SerializeField] private Vector2Int beEliminatedShapeSize;
        [SerializeField] private Vector2Int sizeExpend;


        private SpriteRenderer fogSpriteRender;//房间的迷雾图片
        protected BoxCollider2D roomBox;
        private Texture2D fogTexture;
        private Vector2Int fogDensity;
        private Vector2Int[] shapeLocalPosition;
        private Vector2 worldSize;
        protected bool isGenerate = false;

        public void Generate(CRoomGenerater _generator)
        {
            isGenerate = true;
            roomBox = GetComponent<BoxCollider2D>();
            BoxCollider2D[] colliders = GetComponentsInParent<BoxCollider2D>();
            Vector2 size = roomBox.size;
            foreach (var collider in colliders)
            {
                size.x = Mathf.Max(size.x, collider.size.x);
                size.y = Mathf.Max(size.y, collider.size.y);
            }
            size += sizeExpend;
            roomBox.size = size;
            roomBox.isTrigger = true;

            beEliminatedShapeSize = new Vector2Int(
                Mathf.RoundToInt(beEliminatedShapeSize.x * fogDensityRate), 
                Mathf.RoundToInt(beEliminatedShapeSize.y * fogDensityRate)
                );

            GameObject fog = new GameObject("Fog");
            fog.transform.SetParent(transform);
            fogSpriteRender = fog.AddComponent<SpriteRenderer>();
            fogSpriteRender.sortingLayerName = sortingLayerName;
            fog.transform.localPosition = (Vector3)roomBox.offset;
            gameObject.layer = fogLayerId;
            fog.layer = fogLayerId;

            fogDensity = new Vector2Int(
                Mathf.RoundToInt(roomBox.bounds.size.x * fogDensityRate), 
                Mathf.RoundToInt(roomBox.bounds.size.y * fogDensityRate)
                );
            fogTexture = new Texture2D(fogDensity.x, fogDensity.y);
            Sprite fogSprite = Sprite.Create(fogTexture, new Rect(0, 0, fogDensity.x, fogDensity.y), new Vector2(0.5f, 0.5f), fogDensityRate);
            fogSpriteRender.sprite = fogSprite;
            fogSpriteRender.color = Color.white;
            fogSpriteRender.transform.localScale = Vector3.one;

            worldSize = roomBox.bounds.size;

            InitializeTheShape();
            InitializeTheFog();
        }

        private void Start()
        {
            if(!isGenerate)
            {
                Generate(null);
            }
        }

        private void OnTriggerStay2D(Collider2D _collision)
        {
            if (_collision.GetComponent<IMapPlayer>() != null && _collision.GetComponent<IMapPlayer>().IsAnyKeyInput())
            {
                EliminateFog(_collision.transform.position);
            }       
        }

        private void InitializeTheShape()
        {
            int pixelCount = beEliminatedShapeSize.x * beEliminatedShapeSize.y;
            shapeLocalPosition = new Vector2Int[pixelCount];

            int halfX = Mathf.FloorToInt(beEliminatedShapeSize.x * 0.5f);
            int remainingX = beEliminatedShapeSize.x - halfX;
            int halfY = Mathf.FloorToInt(beEliminatedShapeSize.y * 0.5f);
            int remainingY = beEliminatedShapeSize.y - halfY;

            int index = 0;
            for (int y = -halfY; y < remainingY; y++)
            {
                for (int x = -halfX; x < remainingX; x++)
                {
                    shapeLocalPosition[index] = new Vector2Int(x, y);
                    index++;
                }
            }
        }

        void InitializeTheFog()
        {
            int pixelCount = fogDensity.x * fogDensity.y;
            Color[] blackColors = new Color[pixelCount];
            for (int i = 0; i < pixelCount; i++)
            {
                blackColors[i] = fogColor;
            }
            fogTexture.SetPixels(blackColors);

            fogTexture.Apply();
        }

        void EliminateFog(Vector2 _playerPos)
        {
            //相对假定原点的距离比例,因为是世界坐标,两个点相减有可能是负数,texture中不存在负数的坐标,所以转化为正数.
            Vector2 originDistanceRatio = (_playerPos - (Vector2)roomBox.bounds.min) / worldSize;
            originDistanceRatio.Set(Mathf.Abs(originDistanceRatio.x), Mathf.Abs(originDistanceRatio.y));
            //距离比例乘以密度,玩家在texture中的点即可计算出来
            Vector2Int fogCenter = new Vector2Int(Mathf.RoundToInt(originDistanceRatio.x * fogDensity.x), Mathf.RoundToInt(originDistanceRatio.y * fogDensity.y));
            for (int i = 0; i < shapeLocalPosition.Length; i++)
            {
                int x = shapeLocalPosition[i].x + fogCenter.x;
                int y = shapeLocalPosition[i].y + fogCenter.y;
                //因为消除迷雾的形状是比玩家的位置还要大的,在最边缘的时候,消除的像素点的坐标会超出texture范围,所以超出部分忽略.
                if (x < 0 || x >= fogDensity.x || y < 0 || y >= fogDensity.y)
                    continue;

                fogTexture.SetPixel(x, y, Color.clear);
            }

            fogTexture.Apply();
        }
    }
}

