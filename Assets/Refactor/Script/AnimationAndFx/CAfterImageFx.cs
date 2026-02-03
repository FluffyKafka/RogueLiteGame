using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CAfterImageFx : CEntityAnimFxComponentBase
    {
        [SerializeField] protected float afterImageCooldown;

        protected override void Awake()
        {
            base.Awake();

            animFxSystem.AfterImageBegin += AfterImageBegin;
            animFxSystem.AfterImageEnd += AfterImageEnd;
        }

        protected void AfterImageBegin()
        {

        }

        protected void AfterImageEnd()
        {

        }
    }
}

