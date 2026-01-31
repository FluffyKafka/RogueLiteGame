using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CEntityHitFx_HitFlash : CEntityAnimFxComponentBase
    {
        [SerializeField] protected Material hitMat;
        [SerializeField] protected float fleshDuration = 0.3f;
        protected Material originalMat;

        protected SpriteRenderer sr;

        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr, "CEntityHitFx_HitFlash需要管理一个SpriteRenderer组件");

            animFxSystem.Hit += Hit;
        }

        protected void Hit(WReadOnlyDamageData _data)
        {
            float finalDamage = _data.data.physical + _data.data.magical;
            if(finalDamage <= 0)
            {
                return;
            }

            StartCoroutine(FlashFX());
        }
        protected IEnumerator FlashFX()
        {
            originalMat = sr.material;
            sr.material = hitMat;
            sr.color = Color.white;
            yield return new WaitForSeconds(fleshDuration);
            sr.material = originalMat;
        }
    }
}
