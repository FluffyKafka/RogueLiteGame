using EntitySystem;
using ObjectGenerateData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CEntityAfterImageFx : CEntityAnimFxComponentBase
    {
        protected SpriteRenderer sr;

        [SerializeField] protected float afterImageCooldown;
        [SerializeField] protected float afterImageLifetime;
        [SerializeField] protected float afterImageFadeSmooth;
        protected bool isGenerate = false;

        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();

            animFxSystem.AfterImageBegin += AfterImageBegin;
            animFxSystem.AfterImageEnd += AfterImageEnd;
        }

        protected void AfterImageBegin()
        {
            isGenerate = true;
            StartCoroutine(GenerateAfterImage());
        }

        protected void AfterImageEnd()
        {
            isGenerate = false;
        }

        protected IEnumerator GenerateAfterImage()
        {
            while(isGenerate)
            {
                animFxSystem.EntityGenerateAfterImage(new DAfterImageData(sr.sprite, transform.position, afterImageLifetime, animFxSystem.CheckFacingDir(), afterImageFadeSmooth));
                yield return new WaitForSeconds(afterImageCooldown);
            }
        }
    }
}

