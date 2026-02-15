using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CPlayerAimmingFx : CEntityAnimFxComponentBase
    {
        [SerializeField] protected int dotNum;
        [SerializeField] protected float betweenDotSpace;
        [SerializeField] protected GameObject dotPrefab;

        protected MPlayerAnimationFxSystem playerAnimSystem;
        protected List<GameObject> dots;

        protected override void Awake()
        {
            base.Awake();

            dots = new(dotNum);
            for (int i = 0; i < dotNum; ++i)
            {
                GameObject dot = Instantiate(dotPrefab, transform.position, Quaternion.identity);
                dots.Add(dot);
                dot.SetActive(false);
            }

            playerAnimSystem = animFxSystem as MPlayerAnimationFxSystem;

            playerAnimSystem.AimmingUpdateNotice += AimmingUpdate;
            playerAnimSystem.AimmingFinishNotice += AimmingFinish;
        }

        protected void AimmingUpdate(DProjectileAimmingData _data)
        {
            if (!dots[0].activeSelf)
            {
                foreach(var dot in dots)
                {
                    dot.SetActive(true);
                }
            }

            for (int i = 0; i < dots.Count; ++i)
            {
                dots[i].transform.position = GetPositionByTime(i * betweenDotSpace, _data);
            }
        }
        public Vector2 GetPositionByTime(float _time, DProjectileAimmingData _data)
        {
            Vector2 position =
                (Vector2)transform.position
                + new Vector2(_data.dir.normalized.x * _data.launchSpeed.x, _data.dir.normalized.y * _data.launchSpeed.y)
                * _time + 0.5f * (Physics2D.gravity * _data.gravity) * (_time * _time);
            return position;
        }

        protected void AimmingFinish()
        {
            if (dots[0].activeSelf)
            {
                foreach (var dot in dots)
                {
                    dot.SetActive(false);
                }
            }
        }
    }
}

