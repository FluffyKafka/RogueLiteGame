using EntitySystem.EntityActor;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CEntityHitFx_HitAnim : CEntityAnimFxComponentBase
    {
        [SerializeField] private GameObject hitFXPrefab;
        [SerializeField] private Vector2 maxXYRandomPositionOffset_Hit;
        [SerializeField] private GameObject hitCritFXPrefab;
        [SerializeField] private Vector2 maxXYRandomPositionOffset_CritHit;
        [SerializeField] private float rotateAngle_CritHit;
        protected override void Awake()
        {
            base.Awake();
            animFxSystem.Hit += Hit;
        }

        protected void Hit(WReadOnlyDamageData _data)
        {
            float finalDamage = _data.data.physical + _data.data.magical;
            if(finalDamage <= 0)
            {
                return;
            }

            if(_data.data.isCrit)
            {
                CreateCritHitFX(_data.data.damageSourceTransform);
            }
            else
            {
                CreateHitFX();
            }
        }

        public void CreateHitFX()
        {
            GameObject newHitFX =
                Instantiate(
                    hitFXPrefab,
                    transform.position + new Vector3(
                        Random.Range(-1, 1) * maxXYRandomPositionOffset_Hit.x,
                        Random.Range(-1, 1) * maxXYRandomPositionOffset_Hit.y
                    ),
                    Quaternion.identity,
                    transform
                );
            newHitFX.transform.Rotate(0, 0, Random.Range(0, 180));
            Destroy(newHitFX, 0.5f);
        }

        public void CreateCritHitFX(Transform _damageDir)
        {
            GameObject newHitFX =
                Instantiate(
                    hitCritFXPrefab,
                    transform.position + new Vector3(
                        Random.Range(-1, 1) * maxXYRandomPositionOffset_CritHit.x,
                        Random.Range(-1, 1) * maxXYRandomPositionOffset_CritHit.y
                    ),
                    Quaternion.identity,
                    transform
                );
            newHitFX.transform.Rotate(0, 0, Random.Range(-rotateAngle_CritHit, rotateAngle_CritHit));
            if (_damageDir.position.x > transform.position.x)
            {
                newHitFX.transform.Rotate(0, 180, 0);
            }
            Destroy(newHitFX, 0.5f);
        }
    }
}